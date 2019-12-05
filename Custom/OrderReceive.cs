using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TitleLeader.ServiceReceive;

namespace TitleLeader.Custom
{
    public class OrderReceive
    {
        public ReceiveNoteResponse ReceiveOrder(ReceiveNoteData NoteData)
        {
            ReceiveNoteServiceClient RO = new ReceiveNoteServiceClient();
            RO.ClientCredentials.UserName.UserName = "WWL_XML";
            RO.ClientCredentials.UserName.Password = "Wwl_2019";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            RO.Open();
            return RO.ReceiveNote(NoteData);
        }
    }
}