<?xml version="1.0" encoding="UTF-8"?>
<!-- 

  NxBRE - Index rendering of xBusinessRules 

  Author: David Dossot

-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:strip-space elements="*"/>
	<xsl:param name="title">NxBRE - Rules File Details</xsl:param>
	<xsl:param name="indexfile">bre_index.html</xsl:param>
	<xsl:param name="bodyfile">bre_body.html</xsl:param>
	
	<xsl:template match="/">
		<html>
		<head>
			<title><xsl:value-of select="$title"/></title>
		</head>
		<frameset cols="20%,*">
			<noframes>
				To be viewed properly, this page requires frames.
			</noframes>
			<frame src="{$indexfile}"/>
			<frame src="{$bodyfile}" name="BRE_PSEUDO_CODE_BODY"/>
		</frameset>
		</html>
	</xsl:template>
</xsl:stylesheet>
