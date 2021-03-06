﻿using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [TestFixture]
	[Author("Yufei Liu", "feilfly@gmail.com")]
    [Ignore("please update the unit test to fit your ldap structure")]
    public partial class LdapDirectoryAccountServiceTest : DirectoryAccountServiceTest {

        public LdapDirectoryAccountServiceTest() {
            //rootEntry.AuthenticationType = AuthenticationTypes.None;
            //rootEntry = new DirectoryEntry("LDAP://ldap.company.com/o=company,c=an");
            //DirectoryEntry rootEntry = new DirectoryEntry(this.Url);
            //rootEntry.AuthenticationType = AuthenticationTypes.None;
            service = new LdapDirectoryAccountService();
        }

        [Test]
        public void test_ldap_find_direct_manager() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            DirectoryAccount manager = service.FindDirectManager(user);

            Assert.IsNotNull(manager);
            Assert.AreEqual(user.DirectManager.ToLower(), manager.DistinguishName.ToLower());
        }

        [Test]
        public void test_ldap_find_function_managers() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            IEnumerable<DirectoryAccount> accounts = service.FindFunctionManagers(user);

            Assert.IsNotNull(accounts);
            Assert.AreEqual(0, accounts.Count());
        }

        [Test]
        public void test_ldap_find_team_members() {
            DirectoryAccount user = service.FindByAlias("KLi5");

            IEnumerable<DirectoryAccount> accounts = service.FindTeamMembers(user);

            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Count() > 0);
            Assert.AreEqual(5, accounts.Count());
        }

        [Test]
        public void test_ldap_get_property_value() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            Assert.IsNotNull(user);

            var ldap_user = user as DirectoryAccount;
            var locationorgunit = ldap_user.GetValue("locationorgunit");
            Assert.AreEqual("89039610", locationorgunit);
        }

        protected override void test_ldap_record_yufei(DirectoryAccount user) {
            var msg = this.GetType().ToString() + " unexpected value";
            Assert.IsNotNull(user, this.GetType().ToString() + " - user cannot be null");
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("LYufei", user.Alias, msg);

            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual("Yufei", user.GivenName, msg);
            Assert.AreEqual("Liu", user.SurName, msg);
            Assert.AreEqual("Liu Yufei  100001", user.CommonName, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("IT", user.Organization, msg);
            Assert.AreEqual("Report & Config Systems", user.OrganizationUnit, msg);
            Assert.AreEqual("LYufei@exchange.company.com", user.Mail, msg);
            Assert.AreEqual("cn=Liu Yufei  100001,ou=contractor,o=company,c=an", user.DistinguishName.ToLower(), msg);
            Assert.AreEqual("Yufei Liu", user.DisplayName, msg);
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("04878765", user.GIN, msg);
            Assert.AreEqual("+44 1883 557048", user.TelephoneNumbers[0], msg);
            Assert.AreEqual("Contractor", user.JobTitle, msg);
            Assert.AreEqual("IT", user.Department, msg);
            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual(721668, user.Id, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("RH6 0NZ", user.PostalCode, msg);
            Assert.AreEqual("Buckingham Gate,  West Sussex", user.Street, msg);
            Assert.AreEqual("GB", user.Country, msg);
            Assert.AreEqual("Gatwick", user.City, msg);
            Assert.IsTrue(user.Subscriptions.Length > 0, msg);
            Assert.AreEqual("Horsham", user.ITBuilding, msg);
            Assert.AreEqual("GB0080", user.Geosite, msg);
            //Assert.AreEqual("", user.DirectManagerDirectoryID);


            //information that's not available on AD
            Assert.AreEqual("GB0008", user.LocationCode, msg);
            Assert.AreEqual("82154011", user.JobCode, msg);
            Assert.AreEqual("82154011", user.JobCodeID, msg);
            Assert.AreEqual("JC", user.JobCodeName, msg);
            Assert.AreEqual("IT Operations", user.BusinessCategory, msg);
            Assert.AreEqual(0, user.AccessRights.Length, msg);
            Assert.AreEqual("CN=Liu Yufei  100001,ou=Users,OU=GB,DC=DIR,DC=company,DC=com".ToLower(), user.ActiveDirectoryDN.ToLower(), msg);
            //Assert.AreEqual(3, user.EDMWorkStations.Length);
            Assert.AreEqual("Data Processing", user.JobCategory, msg);
            Assert.AreEqual("00001530", user.JobGroup, msg);
            Assert.AreEqual("JG", user.JobGroupId, msg);
            Assert.AreEqual("JG", user.JobGroupName, msg);
            Assert.AreEqual("", user.AccountingCode, msg);
            Assert.AreEqual("106441", user.AccountingUnit, msg);

        }
    }
}
