using IXM.Constants;
using IXM.Common.Data;
using IXM.Models;
using IXM.Models.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IXM.Common.Constant;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IXM.Common

{
    public class FileService : IFileService
    {
        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private ILogger _logger;
        private List<MSYSTEMS> mSYSTEMS = new List<MSYSTEMS>();

        public GenFunctions genFunctions = new GenFunctions();

        public FileService(IWebHostEnvironment environment, IConfiguration configuration, CustomData customData)
        {
            this._environment = environment;
            this._configuration = configuration;
            mSYSTEMS = customData.GetSystems();
        }



        public async Task <Tuple<int, string, string, int>> SaveDocumentToFileSystem(MOBJECT_DOC model, IFormFile orgimagefile)
        {
            try
            {
                /*var _contentpath = this._environment.WebRootPath;
                var _path = Path.Combine(_contentpath, "Upload");
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                var _ext = Path.GetExtension(orgimagefile.FileName);
                var _allowedext = new string[] { ".jpg", ".png", ".jpeg",".xls",".xlsx" };
                if (!_allowedext.Contains(_ext.ToLower()))
                {
                    string _msg = string.Format("Only {0} extensions are allowed", _allowedext);
                    return new Tuple<int, string, string, int>(0, _msg, _path, 0);

                }

                string _uniquestring = Guid.NewGuid().ToString();
                var _newfilename = _uniquestring + _ext;
                var _filewithpath = Path.Combine(_path, _newfilename);
                var _stream = new FileStream(_filewithpath, FileMode.Create);
                orgimagefile.CopyTo(_stream);
                var _filesize = _stream.Length;
                _stream.Close();
                */

                string _newFilename = Path.ChangeExtension(
                       Path.GetRandomFileName(),
                       Path.GetExtension(orgimagefile.FileName));

                string _ext = Path.GetExtension(orgimagefile.FileName).Remove(1,1);

                string sy = model.SYSTEMID == null ? "0" : model.SYSTEMID;
                var lm = mSYSTEMS.Where(a => a.SYSTEMID == sy).Single();
                string bFolder = _configuration.GetConnectionString("BaseDocFolder");
                IxmAppDocumentType documentType;


                Enum.TryParse(model.DOCTYPE, out documentType);

                var _Path = genFunctions.GetDocumentPath(lm.SYSTEMNAME, documentType, "Live", bFolder);

                if (_Path.Item1 == 0)
                {


                    string path = Path.Combine(
                        _Path.Item2,
                        model.UNAME,
                        _newFilename);

                    Directory.CreateDirectory(Path.Combine(
                        _Path.Item2,
                        model.UNAME));

                    /*var _stream = new FileStream(path, FileMode.Create);
                    orgimagefile.CopyTo(_stream);
                    var _filesize = _stream.Length;*/

                    if (CommonConstants.FileProperties.MaxFileSize >= orgimagefile.Length)
                    {
                        await using FileStream fs = new(path, FileMode.Create);
                        await orgimagefile.OpenReadStream().CopyToAsync(fs);
                    }

                    model.DOCUMENTNAME = _newFilename; //name of image
                    model.SFOLDERNAME = path;
                    model.FILESIZE = orgimagefile.Length;
                    model.FILEXTENSION = _ext.ToUpper();
                    //model.INSERTED_BY = App.

                    return new Tuple<int, string, string, int>(1, _newFilename, _Path.Item2, Convert.ToInt32(model.FILESIZE));


                } else { return new Tuple<int, string, string, int>(_Path.Item1, _newFilename, _Path.Item2, Convert.ToInt32(model.FILESIZE)); }



            }
                catch (Exception ex)
                {

                    return new Tuple<int, string, string, int>(-2, ex.Message, "", -1);

                }
        }


        public Tuple<int, string, string, int> SaveImageFile(MOBJECT_DOC model, IFormFile orgimagefile)
        {
            try
            {
                var _contentpath = this._environment.WebRootPath;
                var _path = Path.Combine(_contentpath, "Upload");
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                var _ext = Path.GetExtension(orgimagefile.FileName);
                var _allowedext = new string[] { ".jpg", ".png", ".jpeg" };
                if (!_allowedext.Contains(_ext.ToLower()))
                {
                    string _msg = string.Format("Only {0} extensions are allowed", _allowedext);
                    return new Tuple<int, string, string, int>(0, _msg, _path, 0);

                }

                string _uniquestring = Guid.NewGuid().ToString();
                var _newfilename = _uniquestring + _ext;
                var _filewithpath = Path.Combine(_path, _newfilename);
                var _stream = new FileStream(_filewithpath, FileMode.Create);
                orgimagefile.CopyTo(_stream);
                var _filesize = _stream.Length;
                _stream.Close();

                model.DOCUMENTNAME = _newfilename; //name of image
                model.SFOLDERNAME = _path;
                model.FILESIZE = _filesize;
                model.FILEXTENSION = _ext.ToUpper();
                //model.INSERTED_BY = App.

                return new Tuple<int, string, string, int>(1, _newfilename, _path, Convert.ToInt32(_filesize));

            }
            catch (Exception ex)
            {

                return new Tuple<int, string, string, int>(-1, ex.Message, "", -1);
            }
        }

        public Tuple<int, byte[], string> GetDocumentData(string pFilename)
        {

            byte[] _data = null;
            try
            {
                //Uri uriAddress2 = new Uri(pFilename);
                //string _File = uriAddress2.LocalPath;


                //if (!File.Exists(_File))
                //{
                //    return new Tuple<int, byte[], string>(-1, null, "File does not exist");
                //}


                _data = File.ReadAllBytes(pFilename);
                //System.Drawing Image _baseimage = 
                //, "image/png", System.IO.Path.GetFileName(pFilename));
                return new Tuple<int, byte[], string>(0, _data, pFilename);


            }
            catch (Exception ex)
            {
                return new Tuple<int, byte[], string>(1, _data, ex.Message);
            }
        }


        public Tuple<int, string> GetImageURI(string pFilename)
        {

            try
            {

                var _pfilename = Path.Combine(_environment.WebRootPath, "Upload");
                if (!File.Exists(_pfilename))
                {
                    return new Tuple<int, string>(-1, "File does not exist");
                }

                return new Tuple<int, string>(0, _pfilename);


            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(1, ex.Message);
            }
        }

        public Tuple<int, string> GetDocumentURI(string pFilename, string pDocType)
        {

            try
            {
                var basepath = _configuration.GetConnectionString("BaseDocFolder");
                //var _pfilename = Path.Combine(_environment.WebRootPath, pDocType);
                var _pfilename = Path.Combine(basepath, pDocType);
                if (!File.Exists(_pfilename))
                {
                    return new Tuple<int, string>(-1, "File does not exist");
                }

                return new Tuple<int, string>(0, _pfilename);


            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(1, ex.Message);
            }
        }



    }
}
