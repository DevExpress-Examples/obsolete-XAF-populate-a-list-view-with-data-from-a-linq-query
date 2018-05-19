# OBSOLETE - How to populate a List View with data from a LINQ query


<p><strong>=====================================</strong><br><strong>Starting from version 16.1, a simpler solution can be implemented based on the <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument114052.aspx">How to: Display a Non-Persistent Object's List View from the Navigation</a> article, using a non-persistent object to represent the query result set.</strong><br><strong>=====================================<br><br>Scenario</strong></p>
<p>Sometimes it is important to show a readonly ListView with custom data obtained from the database, rather than through standard XPO mechanisms, by means of loading entire persistent objects.</p>
<p>While this custom data may include certain properties of your XPO classes, in a general case, it may come from anywhere, including but not limited to, a raw SQL query, stored procedure, database view or a very complex LINQ query returning only a subset of persistent class properties or even custom data fields. While the first scenarios can be solved by <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3281"><u>mapping a persistent class to a database view</u></a>, the second group of scenarios is optimal in implementing a custom form/control (check <a href="https://www.devexpress.com/Support/Center/p/E911">How to show custom forms and controls in XAF (Example)</a> for more details) or a custom <em>CollectionSource</em>. <br> Here, we will demonstrate how the latter option can be realized to display data from a custom LINQ query using the <a href="https://documentation.devexpress.com/#XPO/CustomDocument4060"><u>LINQ to XPO</u></a> approach.</p>
<p>In particular, we want to show a list of employees with maximum orders:</p>
<p><img src="https://raw.githubusercontent.com/DevExpress-Examples/obsolete-how-to-populate-a-list-view-with-data-from-a-linq-query-e859/13.2.5+/media/4ed81ad9-f950-4a9d-8844-77b070542856.png"></p>
<br>
<p><strong>Steps to implement</strong></p>
<p><strong>1.</strong> Include the <em>XAF.LinqToXpo</em> module project in your solution and make sure it builds successfully.</p>
<p>This module is supposed to generate custom ListView nodes in the Application Model with the <strong>_</strong><em>Linq </em>suffix so that this ListView uses data returned by a predefined method (see the <em>XPQueryMethod</em> attribute for the ListView node) using LINQ.</p>
<p>To provide this custom ListView with data, a custom <a href="https://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppCollectionSourceBasetopic"><u>CollectionSourceBase</u></a> class descendant is implemented (see the <em>XAF.LinqToXpo\LinqCollectionSource.xx</em> file). It is enabled for a custom ListView via the <em>XafApplication.CreateCustomCollectionSource</em> event (see the XAF.LinqToXpo\Module.xx file).</p>
<br>
<p><strong>2.</strong> Invoke the <a href="https://documentation.devexpress.com/#Xaf/CustomDocument2828"><u>Module Designer</u></a> for <em>YourSolutionName.Module</em> project and drag and drop the <em>XafLinqToXpoModule</em> component to the <em>Required Modules</em> list (this component should automatically appear in the Visual Studio Toolbox after executing the previous step).</p>
<br>
<p><strong>3.</strong> Implement the public static methods that accept <em>DevExpress.Xpo.Session</em> as a parameter and return <em>System.Linq.IQueryable</em> as a result in your persistent classes, where required.</p>
<p>Consider the following signature for more clarity: <em>public static System.Linq.IQueryable MethodName(DevExpress.Xpo.Session session)</em><em>;</em></p>
<p>Within the method body, use the LINQ to XPO approach to return a required data set based on <em>XPQuery<T></em>. If you want to return a custom property as part of your data set which does not exist within your XPO data model, then include it into the selected part of the query with a custom name (e.g., Employee_Linq, Orders_Sum_Linq, etc.).</p>
<p>Mark these methods with the <em>XAF.LinqToXpo.QueryProjectionsAttribute</em> to specify that comma-separated names of data properties will be included as a query result.</p>
<p>Refer to the <em>NorthwindDemo.Module\PersistentObjects.xx</em> for an example.</p>
<br>
<p><strong>4.</strong> Invoke the Model Editor for <em>YourSoluti</em><em>onName.Module</em> project and add <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3583"><u>custom calculated fields</u></a> with dummy expressions for each specified custom property which is not part of your default XPO data model. Refer to the <em>NorthwindDemo.Module\Model.DesignedDiffs.xafml</em> file for an example.</p>
<br>
<p><strong>IMPORTANT NOTES</strong></p>
<p><strong>1.</strong> This example uses the <em>Northwind </em>database for testing. You can download the database creation scripts for testing <a href="http://technet.microsoft.com/en-us/library/ms143221(v=sql.105).aspx"><u>from here</u></a>.</p>
<p><strong>2.</strong> Since a Linq-based ListView may contain custom data sets and not entire persistent objects in a general case, certain standard Controllers are disabled here (e.g., you cannot open a detail form for this data record or create a new object). Refer to the <em>XAF.LinqToXpo\DisableStandardActionsLinqListViewController.xx</em> file for more details.</p>
<p><strong>3.</strong> If you want your default ListView to include custom calculated values, then it is possible to use the built-in <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3583"><u>Custom Fields</u></a> feature for that purpose.</p>

<br/>


