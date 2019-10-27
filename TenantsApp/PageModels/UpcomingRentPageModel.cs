using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UpcomingRentPageModel: FreshBasePageModel
    {
        ObservableCollection<Rent> rents;
        public ObservableCollection<Rent> Rents
        {
            get { return this.rents; }
            set { this.rents = value; }
        }

        Rent rentSelected;
        public Rent RentSelected
        {
            get
            {
                return this.rentSelected;
            }
            set
            {
                this.rentSelected = value;
            }
        }

        public UpcomingRentPageModel()
        {
            //var aa = typeof(UpcomingRentPageModel).Assembly.FullName;

            //string[] resourceNames = this.GetType().GetTypeInfo().Assembly.GetManifestResourceNames();
            //foreach (var name in resourceNames)
            //{
            //   if(name.Contains("perso"))
            //    {
            //        var asd = "";
            //    }

            //    if (name.Contains("bath"))
            //    {
            //        var asd = "";
            //    }
            //}

            Rents = new ObservableCollection<Rent>();
            var tenant = new Tenant { Name="Paula ateourtua" };
            var place = new Place { Description="Emporium" };
            tenant.Place = place;
            var rent = new Rent { ExpiryDate=DateTime.Now.AddDays(15), Price=145, Tenant= tenant };
            Rents.Add(rent);

             tenant = new Tenant { Name = "Sophie Chung" };
            place = new Place { Description = "Emporium" };
            tenant.Place = place;
            rent = new Rent { ExpiryDate = DateTime.Now.AddDays(7), Price = 145, Tenant = tenant };
            Rents.Add(rent);

            tenant = new Tenant { Name = "Alan" };
            place = new Place { Description = "Berwick" };
            tenant.Place = place;
            rent = new Rent { ExpiryDate = DateTime.Now.AddDays(-5), Price = 150, Tenant = tenant };
            Rents.Add(rent);

            tenant = new Tenant { Name = "Andres" };
            place = new Place { Description = "Berwick" };
            tenant.Place = place;
            rent = new Rent { ExpiryDate = DateTime.Now.AddDays(0), Price = 150, Tenant = tenant };
            Rents.Add(rent);

            tenant = new Tenant { Name = "Camila" };
            place = new Place { Description = "Arthur" };
            tenant.Place = place;
            rent = new Rent { ExpiryDate = DateTime.Now.AddDays(3), Price = 150, Tenant = tenant };
            Rents.Add(rent);

            tenant = new Tenant { Name = "Reem" };
            place = new Place { Description = "Arthur" };
            tenant.Place = place;
            rent = new Rent { ExpiryDate = DateTime.Now.AddDays(-3), Price = 150, Tenant = tenant };
            Rents.Add(rent);

            Rents = new ObservableCollection<Rent>(Rents.OrderBy(x => x.ExpiryDate));
        }
    }
}
