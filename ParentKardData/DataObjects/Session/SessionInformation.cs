﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ParentKardData.DataAccess;

namespace ParentKardData.DataObjects.Session
{
    [Serializable]
    public class SessionInformation : Dictionary<string, object>
    {
        private string CookieName;
        public string SessionID;
        public SessionInformation()
        {
            CookieName = ConfigurationManager.AppSettings["COOKIENAME"].ToString();
        }

        public SessionInformation(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public new void Add(string key, object value)
        {
            base.Add(key, value);
            StoreUpdate();
        }

        public new void Remove(string key)
        {
            base.Remove(key);
            StoreUpdate();
        }

        public new object this[string key]
        {
            get { return base[key]; }
            set
            {
                base[key] = value;
                StoreUpdate();
            }
        }


        private void StoreUpdate()
        {
            if (SessionID != null && SessionID != String.Empty)
            {
                SessionAccess csa = new SessionAccess();
                csa.UpdateSessionData(SessionID, this);
            }
        }

        
        
        
    }
}