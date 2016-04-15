<?xml version="1.0" encoding="UTF-8"?>
<!-- 

  NxBRE - Index rendering of xBusinessRules 

  Author: David Dossot

-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="title">Business Rules Index</xsl:param>
	<xsl:param name="bodyfile">bre_body.html</xsl:param>
	
	<xsl:template match="/">
		<html>
			<head>
				<title><xsl:value-of select="$title"/></title>
				<style type="text/css">
					@media screen { a[href]:hover { background: #ffa } }
					font.standard { font-family: courier; color: black; font-size: 12px }
				</style>
			</head>
			<body bgcolor="white">
				<font class="standard">
					[<a href="{$bodyfile}#RULES_TOP" target="BRE_PSEUDO_CODE_BODY">Top</a>]
					<br/><br/>
					<xsl:apply-templates select="//Set"/>
					<br/>
					[<a href="{$bodyfile}#RULES_BOTTOM" target="BRE_PSEUDO_CODE_BODY">Bottom</a>]
				</font>
			</body>
		</html>
	</xsl:template>
	
	<xsl:template match="Set">
		<a href="{$bodyfile}#SET_{@id}" target="BRE_PSEUDO_CODE_BODY">
				<xsl:value-of select="@id"/>
		</a>
		<br/>
	</xsl:template>
</xsl:stylesheet>
