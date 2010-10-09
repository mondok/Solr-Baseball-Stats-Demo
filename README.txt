1.  Download and install Apache Tomcat
	http://tomcat.apache.org/download-60.cgi

2.  Once installed, copy solr.war into the webapps directory in the Tomcat install folder (check Program Files).
	Be sure that Tomcat is currently not running.

3.  Modify your schema.xml file accordingly.

4.  In the Tomcat Monitor tool on the Java tab, add a new option under Java options.  It will look like this:
	-Dsolr.solr.home=C:\Applications\Testing\SolrCodeCamp\Config
	Note that Config directory will contain a secondary folder called "conf" and within that folder will be your solrconfig.xml
	and schema.xml files.  

5.  Start Tomcat.  

6.  If everything works, you'll see the solr.war file exploded into a directory called "solr" within the Tomcat install folder.

7.  Make sure you set the file and Solr paths accordingly within both the SolrCodeCamp.BaseballWeb project and the 
	SolrCodeCamp.Etl project.

8.  Run the ETL project to fill up the index.

9.  Everything should be set!