// Modified from: https://www.twilio.com/docs/voice/tutorials/how-to-make-outbound-phone-calls/csharp
// See the above page for more help
// Install the C# / .NET helper library from https://twilio.com/docs/csharp/install
// Do this using NuGet. simply type the following command into the Package Manager Console:
//  Install-Package Twilio
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

class Program
{
    static void Main(string[] args)
    {
        // Find your Account SID and Auth Token at twilio.com/console and set the Windows environment variables.
        // See http://twil.io/secure  We recommend you secure access to these variables.
        string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        string senderNumber = Environment.GetEnvironmentVariable("TWILIO_SENDER_NUMBER");

        if (accountSid == null || authToken == null || senderNumber == null)
        {
            Console.WriteLine("Find Account SID, Auth Token and Sender # at twilio.com/console and set");
            Console.WriteLine(" Windows environment variables TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN & TWILIO_SENDER_NUMBER");
            return;
        }
        TwilioClient.Init(accountSid, authToken);
        // Read the text of the message and the phone numbers from the command line
        if (args.Length != 3 || (args[0] != "VOICE" && args[0] != "SMS"))
        {
            Console.WriteLine("FAIL - Invalid arguments. TwilioCallOut \"<type is VOICE or SMS>\" \"<to>\" \"<message>\" ");
            Console.WriteLine("e.g. TwilioCallOut \"VOICE\" \"+4470123456\" \"Hello alarm at site A overrange\" ");
            return;
        }
        // Ensure your From and To phone numbers are registered on the Twilio web site
        Console.WriteLine("Call type: " + args[0]);
        Console.WriteLine("Call to:   " + args[1]);
        Console.WriteLine("Message:   " + args[2]);
        // You can modify the Twiml markup for languages etc.
        switch (args[0] )
        {
            case "VOICE":
                var call = CallResource.Create( // Edit the Twiml for language, voice.
                    twiml: new Twilio.Types.Twiml("<Response><Say>" + args[2] + "</Say></Response>"),
                    to: new Twilio.Types.PhoneNumber(args[1]),
                    from: new Twilio.Types.PhoneNumber(senderNumber)
                );
                Console.WriteLine(call.Sid);
                break;
            case "SMS":
                var message = MessageResource.Create(
                    body: args[2],
                    to: new Twilio.Types.PhoneNumber(args[1]),
                    from: new Twilio.Types.PhoneNumber(senderNumber)
                );
                Console.WriteLine(message.Sid);
                break;
            default:
                Console.WriteLine("Wrong call type " + args[0]);
                break;
        }
        Console.WriteLine("Completed");
    }
}
