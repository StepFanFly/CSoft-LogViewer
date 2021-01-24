using LogViewer.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using LogViewer.ViewModels.Base;

namespace LogViewer.Models
{
    /// <summary>
    /// This is base class for all log files
    /// </summary>
    public class SearhField : ViewModelBase, IEquatable<SearhField>
    {
        #region Public fields
        /// <summary>
        /// This is the slitted string of <see cref="Content"/>
        /// </summary>
        public Dictionary<int, string> SplittedContent { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Collection of filters which allpied to <see cref="SearhField"/>
        /// </summary>
        public ObservableCollectionExt<Filter> Filters { get; set; } = new ObservableCollectionExt<Filter>();

        /// <summary>
        /// This is a child of searched fields
        /// </summary>
        public ObservableCollectionExt<SearhField> ChildSearchedFields { get; set; } = new ObservableCollectionExt<SearhField>();
        
        
        /// <summary>
        /// This is the content of file after read doc
        /// </summary>
        public string Content
        {
            get => content; set 
            {
                content = value;
              
                ViewSting = content;
                SplitContentString();
            } 
        }

        /// <summary>
        /// This is a string to View on Text field
        /// </summary>
        public string ViewSting { get; set; } = new string("");

        /// <summary>
        /// Name of Node
        /// </summary>
        public virtual string NodeName { get; set; }

        /// <summary>
        /// Parent of this item
        /// </summary>
        public SearhField Parent { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Impl of <see cref="IEquatable<SearhField>"/>
        /// </summary>
        /// <param name="other">other search field</param>
        /// <returns></returns>
        public bool Equals([AllowNull] SearhField other)
        {
            return other == null ? false : NodeName.Equals(other.NodeName);
        }

        /// <summary>
        /// Return sel item info
        /// </summary>
        /// <returns>formatted string for side bar</returns>
        public override string ToString()
        {
            return $"{Parent} / Name: {NodeName}";
        }

        /// <summary>
        /// Splits content string
        /// </summary>
        private void SplitContentString()
        {
            SplittedContent.Clear();

            if (string.IsNullOrEmpty(Content))
                return;

            //split the string
            var matches = Regex.Split(Content, @"[\r\n|\r|\n]");

            int nCounter = 0;
            Array.ForEach(matches, curSplittedLine => { SplittedContent.Add(nCounter++, curSplittedLine); });
        }

        #endregion

        #region Members
        private string content;
        #endregion
    }

    /// <summary>
    /// This is a simple log file
    /// </summary>
    public class LogFile : SearhField
    {

        #region Public fields
        /// <summary>
        /// Path to file in disk
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// All information about file
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Name of Node
        /// </summary>
        public override string NodeName { get { return FileInfo == null ? nodeName : FileInfo.Name; } set => nodeName = value; }

        /// <summary>
        /// Indicates loading process
        /// </summary>
        public bool IsBusy { get; set; } = false;
        #endregion

        #region Methods

        /// <summary>
        /// This is the content of file
        /// </summary>
        public async Task ReadFileContentAsync(bool bIsupdate = true)
        {
            await CommandHelper.RunCommandAsync(() => IsBusy, async () =>
            {
                if (File.Exists(FilePath) && bIsupdate)
                {
                    await App.Current.Dispatcher.InvokeAsync(async () => 
                    {
                        Content = await FileUtil.ReadAllTextAsync(FilePath);

                        //TODO: create search fields
                        //example
                        ChildSearchedFields.AddUnique(new SearhField() { NodeName = "Child 1", Content = "bla bla bla 1", Parent = this });
                        ChildSearchedFields.AddUnique(new SearhField() { NodeName = "Child 2", Content = "bla bla bla 2", Parent = this });
                    });
                }
            }, !IsLoadedFile ? 1000 : 0);
        }

        /// <summary>
        /// Return sel item info
        /// </summary>
        /// <returns>formatted string for side bar</returns>
        public override string ToString()
        {
            return FileInfo.Exists ? $"{FileInfo.Name} / {FileInfo.Length / 1024} КБ / {FileInfo.CreationTime}" : FileInfo.Name;
        }
        #endregion

        #region Private members

        private string nodeName;
        private bool IsLoadedFile => !string.IsNullOrEmpty(Content);

        #endregion

    }
}