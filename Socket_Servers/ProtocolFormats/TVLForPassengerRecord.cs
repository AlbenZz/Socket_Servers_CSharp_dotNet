using SocketServers.MessageDEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServers.ProtocolFormats
{


    public class PassengerRecord
    {

        private string passengerNo;
        public string PassengerNo 
        { 
            get 
            {
                return passengerNo; 
            } 
            set 
            { 
                passengerNo = value;
            }
        }
        
        private string personalID;
        public string PersonalID
        { 
            get 
            {
                return personalID; 
            } 
            set 
            {
                personalID = value;
            }
        }
        
        private float onboardLocation;
        public float OnboardLocation
        { 
            get 
            {
                return onboardLocation; 
            } 
            set 
            {
                onboardLocation = value;
            }
        }
        
        private float offboardLocation;
        public float OffboardLocation
        { 
            get 
            {
                return offboardLocation; 
            } 
            set 
            {
                offboardLocation = value;
            }
        }
        
        private string journeyID;
        public string JourneyID
        {
            get
            {
                return journeyID;
            }
            set
            {
                journeyID = value;
            }
        }


        public PassengerRecord(string pn, string pid, float ol, float offl, string jid)
        {
            PassengerNo = pn;
            PersonalID = pid;
            OnboardLocation = ol;
            OffboardLocation = offl;
            JourneyID = jid;
        }

        public PassengerRecord()
        {

        }

        public override string ToString()
        {
            string EOL = "\n";
            return "Passenger NO.  : " + PassengerNo + EOL +
                    "\tPersonal ID          : " + PersonalID + EOL +
                    "\tOnboard Location     : " + OnboardLocation + EOL +
                    "\tOffboardLocation     : " + OffboardLocation + EOL +
                    "\tJourney ID           : " + JourneyID;
        }
    }

    public class TVLForPassengerRecord
    {
        public TVLForPassengerRecord(byte type)
        {
            Type = type;
        }
        public TVLForPassengerRecord(byte type, int length, List<PassengerRecord> pasgRcds)
        {
            Type = type;
            NumberOfPassenger = length;
            PassengerRecords = pasgRcds;
        }
        public byte Type { get; set; }
        public int NumberOfPassenger { get; set; }
        public List<PassengerRecord> PassengerRecords { get; set; }

        public override string ToString()
        {
            string EOLN = "\n";

            string value;
            if (Type == TVLForPassengerRecordConst.FULLREQUEST)
                value = "The message is a request...";
            else if (Type == TVLForPassengerRecordConst.NODATA)
                value = "The message is a null response for a request...";
            else // a data response
            {
                value = "This is a data response for pulling data from the remote server..." + EOLN;
                value += "The response message contains " + NumberOfPassenger + " passenger records::::: " + EOLN + PassengerRecords.ToString();

            }

            return value;
        }
    }
    

}
