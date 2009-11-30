using System;

namespace N2.Installation
{
	/// <summary>
	/// Summarizes database status.
	/// </summary>
	public class DatabaseStatus
	{
		public const int RequiredDatabaseVersion = 2;

		public string ConnectionType { get; set; }
		public int AuthorizedRoles { get; set; }
		public int DetailCollections { get; set; }
		public int Details { get; set; }
		public int Items { get; set; }
		public bool HasSchema { get; set; }
		public bool IsConnected { get; set; }
		public string ConnectionError { get; set; }
		public string SchemaError { get; set; }
		public Exception ConnectionException { get; set; }
		public Exception SchemaException { get; set; }
		public ContentItem StartPage { get; set; }
		public ContentItem RootItem { get; set; }
		public int StartPageID { get; set; }
		public int RootItemID { get; set; }
		public bool IsInstalled { get; set; }
		public string ItemsError { get; set; }
		
		/// <summary>
		/// Known database versions:
		/// 0: no tables
		/// 1: N2 uptil 1.5
		/// 2: N2 from 2.0
		/// </summary>
		public int DatabaseVersion { get; set; }

        public bool NeedsUpgrade { get { return RequiredDatabaseVersion > DatabaseVersion && DatabaseVersion > 0; } }

		public string ToStatusString()
		{
			if (StartPage != null && RootItem != null)
				return string.Format("Everything looks fine, Start page: {0}, Root item: {1}, # of items: {2}",
					 StartPage.Title, RootItem.Title, Items);
			if (RootItem != null)
				return string.Format("There is a root item but couldn't find a start page with the id: {0}", RootItemID);
			if (NeedsUpgrade)
				return string.Format("The database version is {0} but N2 CMS {2} application requires database version {1}", DatabaseVersion, RequiredDatabaseVersion, GetType().Assembly.GetName().Version);
			if (HasSchema)
				return string.Format("The database is installed but there is no content root with the id: {0}", RootItemID);
			if(IsConnected)
				return string.Format(
					"The connection to the database seems fine but the database tables might not be created.<br/>Error message: <pre>{0}</pre>",
					SchemaError);
			
			return string.Format(
				"No database or not properly configured connection string (error message: {0}", 
				ConnectionError);
		}

		public string ToStartPageStatusString()
		{
			return StartPage != null
				? StartPage.Title
				: string.Format("Start page with id {0} not found.</span>", StartPageID);
		}

		public string ToRootItemStatusString()
		{
			return RootItem != null
				? RootItem.Title
				: string.Format("Root item with id {0} not found.</span>", RootItemID);
		}

		public string ToConnectionStatusString()
		{
			return IsConnected
					? "Database connection OK."
					: string.Format("<span title='{0}'>Couldn't open connection to database.</span><!--{1}-->", ConnectionError, ConnectionException);
		}
	}
}
