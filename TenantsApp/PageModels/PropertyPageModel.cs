using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TenantsApp.Entities;

namespace TenantsApp
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PropertyPageModel: FreshBasePageModel
    {
      
        public Place Place { get; set; }

        public PropertyPageModel()
        {

        }

        public override void Init(object initData)
        {
            base.Init(initData);
            this.Place = new Place();

            if (initData != null)
            {
                this.Place = (Place)initData;
            }          
        }
    }
}
