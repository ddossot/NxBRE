<?xml version="1.0" encoding="US-ASCII"?>
<!-- 

  NxBRE - RuleML 0.8 Datalog to Human Readable Format Transformation

  Author: David Dossot
  Modified for RuleML 0.86 by Ron Evans

-->
<xsl:stylesheet version="1.0" xmlns:rml="http://www.ruleml.org/0.86/xsd" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" encoding="US-ASCII"/>
	<xsl:strip-space elements="*"/>
	<xsl:variable name="ucase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
	<xsl:variable name="lcase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
	<xsl:template match="rml:rulebase">
		<xsl:if test="@direction">
			<xsl:text>#DIRECTION_</xsl:text>
			<xsl:value-of select="translate(@direction,$lcase,$ucase)"/>
			<xsl:text disable-output-escaping="yes">&#13;&#10;&#13;&#10;</xsl:text>
		</xsl:if>
		<xsl:apply-templates/>
	</xsl:template>
	<xsl:template match="comment()">
		<xsl:text>/* </xsl:text>
		<xsl:value-of select="."/>
		<xsl:text> */</xsl:text>
		<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:_rbaselab">
		<xsl:text>[</xsl:text>
		<xsl:value-of select="rml:ind/text()"/>
		<xsl:text>];</xsl:text>
		<xsl:text disable-output-escaping="yes">&#13;&#10;&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:imp">
		<xsl:apply-templates select="rml:_rlab"/>
		<xsl:apply-templates select="rml:_body"/>
		<xsl:text disable-output-escaping="yes">&#13;&#10;-> </xsl:text>
		<xsl:apply-templates select="rml:_head"/>
		<xsl:text disable-output-escaping="yes">;&#13;&#10;&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:fact">
		<xsl:apply-templates select="rml:_rlab"/>
		<xsl:text>+</xsl:text>
		<xsl:apply-templates select="rml:_head"/>
		<xsl:text disable-output-escaping="yes">;&#13;&#10;&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:query">
		<xsl:apply-templates select="rml:_rlab"/>
		<xsl:apply-templates select="rml:_body"/>
		<xsl:text disable-output-escaping="yes">;&#13;&#10;&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:and">
		<xsl:apply-templates select="rml:or | rml:naf | rml:atom"/><xsl:text> )</xsl:text>
	</xsl:template>
	<xsl:template match="rml:or">
		<xsl:apply-templates select="rml:and | rml:naf | rml:atom"/><xsl:text> )</xsl:text>
	</xsl:template>
	<xsl:template match="rml:naf">
		<xsl:apply-templates select="rml:atom" mode="naf"/>
	</xsl:template>
	<xsl:template match="rml:atom" mode="naf">
		<xsl:if test="count(../preceding-sibling::*) = 0">
			<xsl:if test="local-name(../../..) = 'and'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&amp; </xsl:text>
			</xsl:if>
			<xsl:if test="local-name(../../..) = 'or'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&#124; </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:if test="count(../preceding-sibling::*) > 0">
			<xsl:if test="local-name(../..) = 'and'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&amp; </xsl:text>
			</xsl:if>
			<xsl:if test="local-name(../..) = 'or'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&#124; </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:if test="count(../preceding-sibling::*) = 0 and count(../following-sibling::*) > 0">( </xsl:if>
		<xsl:text disable-output-escaping="yes">! </xsl:text>
		<xsl:value-of select="rml:_opr/rml:rel/text()"/>
		<xsl:text>{</xsl:text>
		<xsl:apply-templates select="rml:var | rml:ind"/>
		<xsl:text>}</xsl:text>
	</xsl:template>
	<xsl:template match="rml:atom">
		<xsl:if test="count(preceding-sibling::*) = 0">
			<xsl:if test="local-name(../..) = 'and'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&amp; </xsl:text>
			</xsl:if>
			<xsl:if test="local-name(../..) = 'or'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&#124; </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:if test="count(preceding-sibling::*) > 0">
			<xsl:if test="local-name(..) = 'and'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&amp; </xsl:text>
			</xsl:if>
			<xsl:if test="local-name(..) = 'or'">
				<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
				<xsl:text>&#124; </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:if test="count(preceding-sibling::*) = 0 and count(following-sibling::*) > 0">( </xsl:if>
		<xsl:value-of select="rml:_opr/rml:rel/text()"/>
		<xsl:text>{</xsl:text>
		<xsl:apply-templates select="rml:var | rml:ind"/>
		<xsl:text>}</xsl:text>
	</xsl:template>	
	<xsl:template match="rml:_rlab">
		<xsl:text>[</xsl:text>
		<xsl:value-of select="rml:ind/text()"/>
		<xsl:text>]</xsl:text>
		<xsl:text disable-output-escaping="yes">&#13;&#10;</xsl:text>
	</xsl:template>
	<xsl:template match="rml:ind">
		<xsl:if test="position() > 1">, </xsl:if>
		<xsl:value-of select="text()"/>
	</xsl:template>
	<xsl:template match="rml:var">
		<xsl:if test="position() > 1">, </xsl:if>
		<xsl:text>?</xsl:text>
		<xsl:value-of select="text()"/>
	</xsl:template> 
</xsl:stylesheet>
