using System;
using System.Linq;

namespace Us.FolkV3.ApiSample
{
    using Api.Client;
    using Api.Model;
    using Api.Model.Param;

    class Sample
    {
        private readonly HeldinConfig config;
        private PersonSmallClient smallClient;
        private PersonMediumClient mediumClient;
        private PrivateCommunityClient privateCommunityClient;
        private PublicCommunityClient publicCommunityClient;

        public Sample(HeldinConfig config)
        {
            this.config = config;
        }

        static void Main(string[] args)
        {
            // For non secure consumer host:
            //var config = HeldinConfig.ForHost("1.2.3.4")
            //    .Fo().Dev().Com().MemberCode("123456").SubSystemCode("sub-system");
            // For secure consumer host:
            //var config = HeldinConfig.ForSecureHost("1.2.3.4")
            //    .Fo().Dev().Com().MemberCode("123456").SubSystemCode("sub-system");
            // With userId:
            //var config = HeldinConfig.ForSecureHost("1.2.3.4")
            //    .Fo().Dev().Com().MemberCode("123456").SubSystemCode("sub-system")
            //    .WithUserId("user-id");
            // Create Sample instance:
            //		var sample = new Sample(config);
            // Call test methods applicable for your consumer.
        }

        private void Call(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine();
            }
        }

        private void TestSmallMethods()
        {
            Call(TestGetPersonSmallByPrivateId);
            Call(TestGetPersonSmallByPtal);
            Call(TestGetPersonSmallByNameAndAddress);
            Call(TestGetPersonSmallByNameAndDateOfBirth);
        }

        private void TestMediumMethods()
        {
            Call(TestGetPersonMediumByPrivateId);
            Call(TestGetPersonMediumByPublicId);
            Call(TestGetPersonMediumByPtal);
            Call(TestGetPersonMediumByNameAndAddress);
            Call(TestGetPersonMediumByNameAndDateOfBirth);
        }

        private void TestPrivateCommunityMethods()
        {
            Call(TestGetPrivateChanges);
            Call(TestAddPersonToCommunityByNameAndAddress);
            Call(TestAddPersonToCommunityByNameAndDateOfBirth);
            Call(TestRemovePersonFromCommunity);
            Call(TestRemovePersonsFromCommunity);
        }

        private void TestPublicCommunityMethods()
        {
            Call(TestGetPublicChanges);
        }

        private PersonSmallClient SmallClient()
        {
            if (smallClient == null)
            {
                smallClient = FolkClient.PersonSmall(config);
            }
            return smallClient;
        }

        private PersonMediumClient MediumClient()
        {
            if (mediumClient == null)
            {
                mediumClient = FolkClient.PersonMedium(config);
            }
            return mediumClient;
        }

        private PrivateCommunityClient PrivateCommunityClient()
        {
            if (privateCommunityClient == null)
            {
                privateCommunityClient = FolkClient.PrivateCommunity(config);
            }
            return privateCommunityClient;
        }

        private PublicCommunityClient PublicCommunityClient()
        {
            if (publicCommunityClient == null)
            {
                publicCommunityClient = FolkClient.PublicCommunity(config);
            }
            return publicCommunityClient;
        }


        // Test private methods

        private void TestSmallGetMyPrivileges()
        {
            Console.WriteLine("# TestSmallGetMyPrivileges");
            SmallClient().GetMyPrivileges().ToList().ForEach(Console.WriteLine);
        }

        private void TestGetPersonSmallByPrivateId()
        {
            Console.WriteLine("# TestGetPersonSmallByPrivateId");
            var person = SmallClient().GetPerson(
                    PrivateId.Create(1)
                    );
            PrintPerson(person);
        }

        private void TestGetPersonSmallByPtal()
        {
            Console.WriteLine("# TestGetPersonSmallByPtal");
            var person = SmallClient().GetPerson(
                    Ptal.Create("300408559")
                    );
            PrintPerson(person);
        }

        private void TestGetPersonSmallByNameAndAddress()
        {
            Console.WriteLine("# TestGetPersonSmallByNameAndAddress");
            var person = SmallClient().GetPerson(
                    NameParam.Create("Karius", "Davidsen"),
                    AddressParam.Create("Úti í Bø",
                            HouseNumber.Create(16),
                            "Syðrugøta")
                    );
            PrintPerson(person);
        }

        private void TestGetPersonSmallByNameAndDateOfBirth()
        {
            Console.WriteLine("# TestGetPersonSmallByNameAndDateOfBirth");
            var person = SmallClient().GetPerson(
                    NameParam.Create("Karius", "Davidsen"),
                    new DateTime(2008, 4, 30)
                    );
            PrintPerson(person);
        }


        // Test public methods

        private void TestMediumGetMyPrivileges()
        {
            Console.WriteLine("# TestMediumGetMyPrivileges");
            MediumClient().GetMyPrivileges().ToList().ForEach(Console.WriteLine);
        }

        private void TestGetPersonMediumByPrivateId()
        {
            Console.WriteLine("# TestGetPersonMediumByPrivateId");
            var person = MediumClient().GetPerson(
                    PrivateId.Create(1)
                    );
            PrintPerson(person);
        }

        private void TestGetPersonMediumByPublicId()
        {
            Console.WriteLine("# TestGetPersonMediumByPublicId");
            var person = MediumClient().GetPerson(
                    PublicId.Create(1157442)
                    );
            PrintPerson(person);
        }

        private void TestGetPersonMediumByPtal()
        {
            Console.WriteLine("# TestGetPersonMediumByPtal");
            var person = MediumClient().GetPerson(
                    Ptal.Create("300408559")
                    );
            PrintPerson(person);
        }

        private void TestGetPersonMediumByNameAndAddress()
        {
            Console.WriteLine("# TestGetPersonMediumByNameAndAddress");
            var person = MediumClient().GetPerson(
                    NameParam.Create("Karius", "Davidsen"),
                    AddressParam.Create("Úti í Bø",
                            HouseNumber.Create(16),
                            "Syðrugøta")
                    );
            PrintPerson(person);
        }

        private void TestGetPersonMediumByNameAndDateOfBirth()
        {
            Console.WriteLine("# TestGetPersonMediumByNameAndDateOfBirth");
            var person = MediumClient().GetPerson(
                    NameParam.Create("Karius", "Davidsen"),
                    new DateTime(2008, 4, 30)
                    );
            PrintPerson(person);
        }


        // Test community methods

        private void TestGetPrivateChanges()
        {
            Console.WriteLine("# TestGetPrivateChanges");
            Changes<PrivateId> changes = PrivateCommunityClient().GetChanges(DateTime.Now.AddDays(-7));
            Console.WriteLine("Changes - from: {0}; to: {1}; ids: [{2}]\n", changes.From, changes.To, string.Join(", ", changes.Ids));
        }

        private void TestGetPublicChanges()
        {
            Console.WriteLine("# TestGetPublicChanges");
            Changes<PublicId> changes = PublicCommunityClient().GetChanges(DateTime.Now.AddDays(-7));
            Console.WriteLine("Changes - from: {0}; to: {1}; ids: [{2}]\n", changes.From, changes.To, string.Join(", ", changes.Ids));
        }

        private void TestAddPersonToCommunityByNameAndAddress()
        {
            Console.WriteLine("# TestAddPersonToCommunityByNameAndAddress");
            var communityPerson = PrivateCommunityClient().AddPersonToCommunity(
                    NameParam.Create("Karius", "Davidsen"),
                    AddressParam.Create("Úti í Bø",
                            HouseNumber.Create(16),
                            "Syðrugøta")
                    );
            PrintCommunityPerson(communityPerson);
        }

        private void TestAddPersonToCommunityByNameAndDateOfBirth()
        {
            Console.WriteLine("# TestAddPersonToCommunityByNameAndDateOfBirth");
            var communityPerson = PrivateCommunityClient().AddPersonToCommunity(
                    NameParam.Create("Karius", "Davidsen"),
                    new DateTime(2008, 4, 30)
                    );
            PrintCommunityPerson(communityPerson);
        }

        private void TestRemovePersonFromCommunity()
        {
            Console.WriteLine("# TestRemovePersonFromCommunity");
            var removedId = PrivateCommunityClient().RemovePersonFromCommunity(PrivateId.Create(1));
            Console.WriteLine("Removed id: {0}\n", removedId);
        }

        private void TestRemovePersonsFromCommunity()
        {
            Console.WriteLine("# TestRemovePersonsFromCommunity");
            var removedIds = PrivateCommunityClient().RemovePersonsFromCommunity(PrivateId.Create(1, 2, 3));
            Console.WriteLine("Removed ids: [{0}]\n", string.Join(", ", removedIds));
        }


        // Print methods

        private static void PrintPerson(PersonSmall person)
        {
            if (person == null)
            {
                Console.WriteLine("Person was not found!");
            }
            else
            {
                Console.WriteLine(PersonToString(person));
            }
            Console.WriteLine();
        }

        private static void PrintCommunityPerson(CommunityPerson person)
        {
            if (person == null)
            {
                Console.WriteLine("Oops!");
            }
            else
            {
                Console.WriteLine(CommunityPersonToString(person));
            }
            Console.WriteLine();
        }

        private static String PersonToString(PersonSmall person)
        {
            if (person.GetType() == typeof(PersonMedium)) {
                var personPublic = (PersonMedium)person;
                return Format(person.PrivateId, personPublic.PublicId, person.Name, AddressToString(person),
                        personPublic.DateOfBirth,
                        CivilStatusToString(personPublic), SpecialMarksToString(personPublic),
                        IncapacityToString(personPublic));
            }
            var deadOrAlive = person.IsAlive ? "ALIVE" : ("DEAD " + person.DateOfDeath);
            return Format(person.PrivateId, person.Name, AddressToString(person), deadOrAlive);
        }

        private static String CommunityPersonToString(CommunityPerson communityPerson)
        {
            String personString = null;
            if (communityPerson.IsAdded)
            {
                personString = PersonToString(communityPerson.Person);
            }
            return Format(communityPerson.Status, communityPerson.ExistingId, personString);
        }

        private static String AddressToString(PersonSmall person)
        {
            return AddressToString(person.Address);
        }

        private static String AddressToString(Address address)
        {
            return address.HasStreetAndNumbers
                            ? address.StreetAndNumbers
                                    + "; " + address.Country.Code + address.PostalCode
                                    + " " + address.City
                                    + "; " + address.Country.NameFo
                                    + " (From: " + address.From + ')'
                            : null;
        }

        private static String CivilStatusToString(PersonMedium person)
        {
            if (person.CivilStatus == null)
            {
                return null;
            }
            return person.CivilStatus.Type + ", " + person.CivilStatus.From;
        }

        private static String SpecialMarksToString(PersonMedium person)
        {
            return person.SpecialMarks.IsEmpty
                    ? null : ('[' + string.Join(", ", person.SpecialMarks.Select(s => s.ToString())) + ']');
        }

        private static String IncapacityToString(PersonMedium person)
        {
            if (person.Incapacity == null)
            {
                return null;
            }
            var guardian1 = GuardianToString(person.Incapacity.Guardian1);
            var guardian2 = GuardianToString(person.Incapacity.Guardian2);
            return guardian2 == null ? guardian1 : guardian1 + " / " + guardian2;
        }

        private static String GuardianToString(Guardian guardian)
        {
            if (guardian == null)
            {
                return null;
            }
            return guardian.Name + " - " + AddressToString(guardian.Address);
        }

        private static String Format(params Object[] values)
        {
            return string.Join(
                " | ",
                values.ToList().Select(v => v == null ? "-" : v.ToString())
                );
        }

    }
}
