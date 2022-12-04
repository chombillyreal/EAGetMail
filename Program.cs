using System;
using System.Globalization;
using System.IO;
using EAGetMail;

namespace Client {
    class Program {
        public static void Main(string[] args) {
            try {
                string dizin = string.Format("{0}//dizin", Directory.GetCurrentDirectory());
                if (!Directory.Exists(dizin))
                {
                    Directory.CreateDirectory(dizin);
                }
                MailServer oServer = new MailServer("imap.gmail.com", "yourmail", "yourpassword", ServerProtocol.Imap4);
                oServer.SSLConnection = true;
                oServer.Port = 993;

                MailClient oClient = new MailClient("TryIt");
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos(); 
                int count = infos.Length;
                Console.WriteLine(count);
                if(count != 0) {
                    for(int i=0;i<infos.Length;i++) {
                        MailInfo info = infos[i];
                        Mail oMail = oClient.GetMail(info);
                        string subjectDizin = string.Format("{0}//dizin//{1}", Directory.GetCurrentDirectory(), oMail.Subject);
                        if(!Directory.Exists(subjectDizin)) {
                            Directory.CreateDirectory(subjectDizin);
                        }
                        string contentDizin = string.Format("{0}//dizin//{1}//{2}", Directory.GetCurrentDirectory(), oMail.Subject , "content.txt");
                        if(!File.Exists(contentDizin)) {
                            using (StreamWriter sw = File.CreateText(contentDizin)) {
                                sw.WriteLine(oMail.From.ToString());
                                sw.WriteLine(oMail.HtmlBody);
                            }
                           
                        }
                        
                        Attachment [] atts = oMail.Attachments;
                        for(int j=0;j<atts.Length;j++) {
                            Attachment att = atts[j];
                            string attPath = string.Format("{0}//dizin//{1}//{2}", Directory.GetCurrentDirectory(), oMail.Subject, att.Name);
                            att.SaveAs(attPath, true);
                        }
                    }
                }
                oClient.Quit();
            } catch(Exception ep) {
                Console.WriteLine(ep.Message);
            }
            Console.WriteLine("Mailler indirildi.");

            Console.ReadLine();
        }
    }
}

