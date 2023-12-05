using System.IO.Compression;
using ProjectTools.Core.Implementations;
using ProjectTools.Core.Options;
using ProjectTools.Core.Templating.Common;

namespace ProjectTools.Core.Templating.Preparation
{
    /// <summary>
    /// An abstract templater.
    /// </summary>
    public abstract class AbstractTemplatePreparer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTemplatePreparer"/> class.
        /// </summary>
        /// <param name="longName">The long name.</param>
        /// <param name="implementation">The implementation.</param>
        public AbstractTemplatePreparer(string longName, Implementation implementation)
        {
            LongName = longName;
            Implementation = implementation;
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>The implementation.</value>
        public Implementation Implementation { get; }

        /// <summary>
        /// Gets the long name of the templater.
        /// </summary>
        /// <value>The long name.</value>
        public string LongName { get; }

        /// <summary>
        /// Gets the short name of the templater.
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName => Implementation.ToString().ToLowerInvariant();

        /// <summary>
        /// Checks whether the directory is valid for this templater.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True if valid, False otherwise.</returns>
        public abstract bool DirectoryValidForTemplater(string directory);

        /// <summary>
        /// Gets a list of settings that need to be set to generate a template of this type.
        /// </summary>
        /// <returns>The list of needed settings.</returns>
        public List<SettingProperty> GetSettingProperties()
        {
            var type = GetTemplateSettingsClass();

            if (type == null)
            {
                throw new Exception("Template settings class not defined for this templater.");
            }

            var fields = type.GetFields();
            var output = new List<SettingProperty>();

            foreach (var field in fields)
            {
                output.Add(new SettingProperty() { Name = field.Name, Type = field.FieldType, });
            }

            return output;
        }

        /// <summary>
        /// Gets a list of settings that need to be set to generate a template of this type.
        /// </summary>
        /// <returns>The list of needed settings.</returns>
        public abstract TemplateSettings GetSettingsClassForProperties(Dictionary<SettingProperty, object> settings);

        /// <summary>
        /// Gets the template settings class.
        /// </summary>
        /// <returns>The template settings class defined for this preparer.</returns>
        public abstract Type? GetTemplateSettingsClass();

        /// <summary>
        /// Prepares the template.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The preperation result.</returns>
        public abstract string Prepare(PrepareOptions options, Func<string, bool> log);

        /// <summary>
        /// Archives a directory.
        /// </summary>
        /// <param name="directoryToArchive">The directory to archive.</param>
        /// <param name="archivePath">The archive path.</param>
        /// <param name="skipCleaning">if set to <c>true</c> [skip cleaning].</param>
        protected void ArchiveDirectory(string directoryToArchive, string archivePath, bool skipCleaning)
        {
            // Delete any existing archive
            if (File.Exists(archivePath))
            {
                File.Delete(archivePath);
            }

            // Create a new archive
            ZipFile.CreateFromDirectory(directoryToArchive, archivePath);

            // Delete the base directory to archive if requested
            if (!skipCleaning)
            {
                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        Directory.Delete(directoryToArchive, true);
                        break;
                    }
                    catch (Exception)
                    {
                        // Give some time to let the OS release the directory
                        Thread.Sleep(500);
                    }
                }
            }
        }

        /// <summary>
        /// Copies a directory.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        /// <param name="excludedDirectories">The excluded directories.</param>
        protected void CopyDirectory(string oldPath, string newPath, List<string> excludedDirectories)
        {
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            // Copy over items
            var files = Directory.GetFiles(oldPath);
            for (var i = 0; i < files.Length; i++)
            {
                var path = Path.Combine(newPath, Path.GetFileName(files[i]));
                File.Copy(files[i], path, true);
            }

            // Copy over sub-directories
            var directories = Directory.GetDirectories(oldPath);
            for (var i = 0; i < directories.Length; i++)
            {
                var dirName = Path.GetFileName(directories[i]);
                if (!excludedDirectories.Contains(dirName.ToLowerInvariant()))
                {
                    var path = Path.Combine(newPath, dirName);
                    CopyDirectory(directories[i], path, excludedDirectories);
                }
            }
        }

        /// <summary>
        /// Deletes the directory if exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        protected bool DeleteDirectoryIfExists(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the file if exists.
        /// </summary>
        /// <param name="file">The file.</param>
        protected bool DeleteFileIfExists(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }

            return false;
        }
    }
}
