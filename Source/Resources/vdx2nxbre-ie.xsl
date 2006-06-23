<?xml version="1.0" encoding="UTF-8"?>
<!-- 

  NxBRE - Visio 2003 VDX to Slotless RuleML 0.86 Naf Datalog Transformation
				- Version 2.5.1

  Author: David Dossot

-->
<xsl:stylesheet version="1.0" exclude-result-prefixes="vdx" xmlns="http://www.ruleml.org/0.86/xsd" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:vdx="http://schemas.microsoft.com/visio/2003/core">
	<xsl:param name="selected-pages"/>
	<xsl:output indent="yes" method="xml" encoding="utf-8" />
	
	<xsl:variable name="MID-implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Implication']/@ID"/>
	<xsl:variable name="MID-negative.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:NegativeImplication']/@ID"/>
	<xsl:variable name="MID-counting.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:CountingImplication']/@ID"/>
	<xsl:variable name="MID-modifying.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:ModifyingImplication']/@ID"/>
	<xsl:variable name="MID-fact" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Fact']/@ID"/>
	<xsl:variable name="MID-query" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Query']/@ID"/>
	
	<xsl:template match="/">
		<rulebase direction="forward" xsi:schemaLocation="http://www.ruleml.org/0.86/xsd ruleml-0_86-nafdatalog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
			<xsl:if test="/vdx:VisioDocument/vdx:DocumentProperties/vdx:Title">
				<_rbaselab>
					<ind>
						<xsl:value-of select="/vdx:VisioDocument/vdx:DocumentProperties/vdx:Title/text()"/>
					</ind>
				</_rbaselab>
			</xsl:if>
			
			<xsl:comment> Implications "<xsl:value-of select="$selected-pages"/>" </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-implication]" mode="implication">
						<xsl:with-param name="action">assert</xsl:with-param>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-implication]" mode="implication">
						<xsl:with-param name="action">assert</xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
			
			<xsl:comment> Negative Implications "<xsl:value-of select="$selected-pages"/>" </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-negative.implication]" mode="implication">
						<xsl:with-param name="action">retract</xsl:with-param>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-negative.implication]" mode="implication">
						<xsl:with-param name="action">retract</xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
			
			<xsl:comment> Counting Implications "<xsl:value-of select="$selected-pages"/>" </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-counting.implication]" mode="implication">
						<xsl:with-param name="action">count</xsl:with-param>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-counting.implication]" mode="implication">
						<xsl:with-param name="action">count</xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
			
			<xsl:comment> Modifying Implications "<xsl:value-of select="$selected-pages"/>" </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-modifying.implication]" mode="implication">
						<xsl:with-param name="action">modify</xsl:with-param>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-modifying.implication]" mode="implication">
						<xsl:with-param name="action">modify</xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
			
			<xsl:comment> Facts </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-fact]" mode="fact"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-fact]" mode="fact"/>
				</xsl:otherwise>
			</xsl:choose>
			
			<xsl:comment> Queries </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-query]" mode="query"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-query]" mode="query"/>
				</xsl:otherwise>
			</xsl:choose>
		</rulebase>
	</xsl:template>
	
	<!-- Do not process any shape unless specifically targeted  -->
	<xsl:template match="vdx:Shape"/>
	
	<xsl:template match="vdx:Shape" mode="implication">
		<xsl:param name="action"/>
		<imp>
			<xsl:call-template name="implication.label">
				<xsl:with-param name="action" select="$action"/>
			</xsl:call-template>
			<_head>
				<xsl:call-template name="atom"/>
			</_head>
			<_body>
				<xsl:call-template name="connector-start"/>
			</_body>
		</imp>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="fact">
		<fact>
			<xsl:call-template name="label"/>
			<_head>
				<xsl:call-template name="atom"/>
			</_head>
		</fact>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="query">
		<query>
			<xsl:call-template name="label"/>
			<_body>
				<xsl:call-template name="connector-start"/>
			</_body>
		</query>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:NafAtom']/@ID]">
		<naf>
			<xsl:call-template name="atom"/>
		</naf>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Atom']/@ID]">
		<xsl:call-template name="atom"/>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:And']/@ID]">
		<xsl:param name="parent-ID"/>
		<and>
			<xsl:call-template name="connector-start">
				<xsl:with-param name="parent-ID" select="$parent-ID"/>
			</xsl:call-template>
		</and>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Or']/@ID]">
		<xsl:param name="parent-ID"/>
		<or>
			<xsl:call-template name="connector-start">
				<xsl:with-param name="parent-ID" select="$parent-ID"/>
			</xsl:call-template>
		</or>
	</xsl:template>
	
	<xsl:template name="connector-start">
		<xsl:param name="parent-ID" select="@ID"/>
		<xsl:variable name="page-context" select="../.."/>
		<xsl:variable name="this-ID" select="@ID"/>
		
		<xsl:for-each select="$page-context/vdx:Connects/vdx:Connect[@FromSheet=$page-context/vdx:Connects/vdx:Connect[@ToSheet=$this-ID]/@FromSheet and @ToSheet!=$this-ID and @ToSheet!=$parent-ID]">
			<xsl:variable name="end-ID" select="string(./@ToSheet)"/>
			<xsl:apply-templates select="$page-context/vdx:Shapes/vdx:Shape[@ID=$end-ID]">
				<xsl:with-param name="parent-ID" select="$this-ID"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="atom">
		<xsl:variable name="MasterID" select="@Master"/>
		<atom>
			<_opr>
				<rel>
					<xsl:call-template name="nxbre-operators">
						<xsl:with-param name="predicate-string">
							<xsl:choose>
								<xsl:when test="not(vdx:Shapes/vdx:Shape[@Name='relation']/vdx:Text)">
									<!-- The relation is not defined in the shape, let's look into the master -->
									<xsl:value-of select="normalize-space(/vdx:VisioDocument/vdx:Masters/vdx:Master[@ID=$MasterID]//vdx:Shape[@Name='relation']/vdx:Text/text())"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(vdx:Shapes/vdx:Shape[@Name='relation']/vdx:Text/text())"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:with-param>
					</xsl:call-template>
				</rel>
			</_opr>
			<xsl:call-template name="predicates">
				<xsl:with-param name="predicate-list">
					<xsl:choose>
						<xsl:when test="not(vdx:Shapes/vdx:Shape[@Name='predicates']/vdx:Text)">
									<!-- The predicates are not defined in the shape, let's look into the master -->
							<xsl:apply-templates mode="trim-string" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[@ID=$MasterID]//vdx:Shape[@Name='predicates']/vdx:Text//text()"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates mode="trim-string" select="vdx:Shapes/vdx:Shape[@Name='predicates']/vdx:Text//text()"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</atom>
	</xsl:template>
	
	<xsl:template name="nxbre-operators">
		<xsl:param name="predicate-string"/>
		<xsl:choose>
			<xsl:when test="starts-with($predicate-string, '>=')">NxBRE:GreaterThanEqualTo(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '&lt;=')">NxBRE:LessThanEqualTo(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '&lt;>') or starts-with($predicate-string, '!=')">NxBRE:NotEquals(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '==')">NxBRE:Equals(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '>')">NxBRE:GreaterThan(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '&lt;')">NxBRE:LessThan(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
			<xsl:when test="starts-with($predicate-string, '=')">NxBRE:Equals(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$predicate-string"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="predicate">
		<xsl:param name="predicate-string"/>
		<xsl:choose>
			<xsl:when test="starts-with($predicate-string, '?')">
				<var>
					<xsl:value-of select="normalize-space(substring-after($predicate-string, '?'))"/>
				</var>
			</xsl:when>
			<xsl:otherwise>
				<ind>
					<xsl:call-template name="nxbre-operators">
						<xsl:with-param name="predicate-string" select="normalize-space($predicate-string)"/>
					</xsl:call-template>
				</ind>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="predicates">
		<xsl:param name="predicate-list"/>
		<xsl:if test="string-length($predicate-list)">
			<xsl:choose>
				<xsl:when test="contains($predicate-list, '&#x0a;')">
					<xsl:call-template name="predicate">
						<xsl:with-param name="predicate-string" select="substring-before($predicate-list, '&#x0a;')"/>
					</xsl:call-template>
					<xsl:call-template name="predicates">
						<xsl:with-param name="predicate-list" select="substring-after($predicate-list, '&#x0a;')"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="predicate">
						<xsl:with-param name="predicate-string" select="$predicate-list"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="label">
		<xsl:if test="vdx:Prop[vdx:Label='Label']">
			<_rlab>
				<ind>
					<xsl:value-of select="vdx:Prop[vdx:Label='Label']/vdx:Value/text()"/>
				</ind>
			</_rlab>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="implication.label">
		<xsl:param name="action"/>
		<_rlab>
			<ind>
				<xsl:text>label:</xsl:text>
				<xsl:variable name="label" select="vdx:Prop[vdx:Label='Label']/vdx:Value/text()"/>
				<xsl:choose>
					<xsl:when test="$label!=''"><xsl:value-of select="$label"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="generate-id(.)"/></xsl:otherwise>
				</xsl:choose>
				<xsl:text>;priority:</xsl:text>
				<xsl:value-of select="vdx:Prop[vdx:Label='Priority']/vdx:Value/text()"/>
				<xsl:text>;mutex:</xsl:text>
				<xsl:value-of select="vdx:Prop[vdx:Label='Mutex']/vdx:Value/text()"/>
				<xsl:text>;precondition:</xsl:text>
				<xsl:value-of select="vdx:Prop[vdx:Label='Precondition']/vdx:Value/text()"/>
				<xsl:text>;action:</xsl:text>
				<xsl:value-of select="$action"/>
				<xsl:text>;</xsl:text>
			</ind>
		</_rlab>
	</xsl:template>
</xsl:stylesheet>
