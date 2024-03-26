using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            return await Task.Run<string>(() =>
            {
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = string.Empty;
                do
                {
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}-{DateTime.Now.ToString("ddMMyyyyHHmmsss")}{extension}";

                } while (hasFileMethod(pathOrContainerName, newFileName));
                //} while (File.Exists($"{path}\\{newFileName}"));

                return newFileName;
            });
        }
    }
}
