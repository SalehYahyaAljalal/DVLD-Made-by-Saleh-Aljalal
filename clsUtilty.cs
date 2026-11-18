using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Win32;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLDProject
{
    public static class clsUtilty
    {


        //public static bool CreateFolderIfDoesNotExists(string FolderName)
        //{
        //    if(!Directory.Exists(FolderName))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(FolderName);
        //            return true;

        //        }
        //        catch (IOException IO)
        //        {
        //            return false;
        //        }

        //    }

        //    return true;

        //}

        //public static  string GenerateGuid()
        //{
        //    Guid guid = Guid.NewGuid();
        //    return guid.ToString();
        //}

        //public static string ReplaceFileNameWithGUID(string SourceImage)
        //{
        //    FileInfo SourceFile = new FileInfo(SourceImage);
        //    string ETEX = SourceFile.Extension;

        //    return  GenerateGuid() + ETEX; 

        //}

        //public static bool CopyImageToProjectImagese(ref string SourceImage)
        //{
        //    string DestinationFolder = @"C:\Users\DELL\Desktop\DVLDProject\ImagesPeople\";

        //    if(!CreateFolderIfDoesNotExists(DestinationFolder))
        //    {
        //        return false;
        //    }


        //    string DestinationFile= DestinationFolder + ReplaceFileNameWithGUID(SourceImage);

        //    try
        //    {
        //        File.Copy(SourceImage, DestinationFile, true);

        //    }
        //    catch (IOException)
        //    {
        //        return false;
        //    }

        //    SourceImage = DestinationFile;
        //    return true;


        //}



        public static bool RememberUserNameAndPassword(string UserName,string Password)
        {
            //string CurrentDirectory=Directory.GetCurrentDirectory();
            ////  string FilePath = CurrentDirectory + "\\Data.txt";
           

            string UserCredentionalsPath = "HKEY_CURRENT_USER\\Software\\YourNameKey";
            string UserCredentionalsFile = "UserCredintioanls";

            string UserCrendintionalData = UserName + "#//#" + Password;

            try
            {

                Registry.SetValue(UserCredentionalsPath, UserCredentionalsFile, UserCrendintionalData, RegistryValueKind.String);
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public static bool DeleteUserCredentioalsInfo()
        {
            string UserCredentionalsPath = @"Software\YourNameKey";
            string UserCredentionalsFile = "UserCredintioanls";

            try
            {
                using (RegistryKey BaseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {

                    using (RegistryKey SubKey=BaseKey.OpenSubKey(UserCredentionalsPath,true))
                    {
                        if(SubKey != null)
                        {
                            SubKey.DeleteValue(UserCredentionalsFile);
                            return true;
                        }


                    }

                }




            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public static bool GetCredentioals(ref string UserName,ref string Password)
        {
            //string CurrentDirectory = Directory.GetCurrentDirectory();
            //string FilePath = CurrentDirectory + "\\Data.txt";

            string UserCredentionalsPath = "HKEY_CURRENT_USER\\Software\\YourNameKey";
            string UserCredentionalsFile = "UserCredintioanls";

            try
            {
          
                string userCredentionals = Registry.GetValue(UserCredentionalsPath, UserCredentionalsFile, null) as string;

                if(userCredentionals != null)
                {

                    string[] Data = userCredentionals.Split(new string[] { "#//#" }, StringSplitOptions.None );

                    UserName = Data[0];
                    Password = Data[1];


                }


                return true;




            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public static string GenerateGuit()
        {
            Guid guid= Guid.NewGuid();
            return guid.ToString(); 
        }

       public static string ReplaceImageNameWithGuid(string SourceImage)
        {
            FileInfo fileInfo=new FileInfo(SourceImage);


            return GenerateGuit() + fileInfo.Extension;

        }

        public static bool CreateFolderDeosNotExists(string SourecFolder)
        {
            if(!Directory.Exists(SourecFolder))
            {
                try
                {
                    Directory.CreateDirectory(SourecFolder);
                    return true;

                }catch(Exception Ex)
                {
                    return false;   
                }

            }
            return true;

        }
        public static bool CopyImageToFolderProject(ref string SourceImage)
        {
            string FolderPath = @"C:\Users\DELL\Desktop\DVLDProject\ImagesPeople\";

            if(!CreateFolderDeosNotExists(FolderPath))
            {
                return false;
            }

            string destinationfil = FolderPath + ReplaceImageNameWithGuid(SourceImage);

            try
            {
                File.Copy(SourceImage, destinationfil,true);

            }catch(Exception Ex)
            {
                return false;
            }

            SourceImage = destinationfil;
            return true;

        }





    }
}
