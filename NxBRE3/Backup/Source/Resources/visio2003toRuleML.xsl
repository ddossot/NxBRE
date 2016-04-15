<?xml version="1.0" encoding="UTF-8"?>
<!-- 

  NxBRE - Visio 2003 VDX to RuleML 0.9 Naf Datalog Transformation

  Author: David Dossot

-->
<xsl:stylesheet version="1.0" exclude-result-prefixes="vdx"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
								xmlns="http://www.ruleml.org/0.9/xsd"
								xmlns:vdx="http://schemas.microsoft.com/visio/2003/core">
	<xsl:param name="selected-pages"/>
	<xsl:param name="strict"/>
	<xsl:output indent="yes" method="xml" encoding="utf-8" />
	
	<xsl:variable name="MID-implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Implication']/@ID"/>
	<xsl:variable name="MID-negative.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:NegativeImplication']/@ID"/>
	<xsl:variable name="MID-counting.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:CountingImplication']/@ID"/>
	<xsl:variable name="MID-modifying.implication" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:ModifyingImplication']/@ID"/>
	<xsl:variable name="MID-fact" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Fact']/@ID"/>
	<xsl:variable name="MID-query" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Query']/@ID"/>
	<xsl:variable name="MID-integrity.query" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Integrity']/@ID"/>
	<xsl:variable name="MID-equivalent" select="/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Equivalent']/@ID"/>
	
	<xsl:template match="/">
		<RuleML xsi:schemaLocation="http://www.ruleml.org/0.9/xsd ruleml-0_9-nafdatalog.xsd" xmlns:xs="http://www.w3.org/2001/XMLSchema">
			<xsl:if test="/vdx:VisioDocument/vdx:DocumentProperties/vdx:Title">
				<oid>
					<Ind>
						<xsl:value-of select="/vdx:VisioDocument/vdx:DocumentProperties/vdx:Title/text()"/>
					</Ind>
				</oid>
			</xsl:if>
			
			<Assert>
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
			
				<xsl:comment> Equivalents </xsl:comment>
				<xsl:choose>
					<xsl:when test="$selected-pages != ''">
						<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-equivalent]" mode="equivalent"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-equivalent]" mode="equivalent"/>
					</xsl:otherwise>
				</xsl:choose>
			</Assert>
			
			<xsl:comment> Integrity Queries </xsl:comment>
			<xsl:choose>
				<xsl:when test="$selected-pages != ''">
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page[contains($selected-pages, concat('|', @Name, '|'))]/vdx:Shapes/vdx:Shape[@Master=$MID-integrity.query]" mode="integrity"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="/vdx:VisioDocument/vdx:Pages/vdx:Page/vdx:Shapes/vdx:Shape[@Master=$MID-integrity.query]" mode="integrity"/>
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
		</RuleML>
	</xsl:template>
	
	<!-- Do not process any shape unless specifically targeted  -->
	<xsl:template match="vdx:Shape"/>
	
	<xsl:template match="vdx:Shape" mode="implication">
		<xsl:param name="action"/>
		<Implies>
			<xsl:call-template name="implication.label">
				<xsl:with-param name="action" select="$action"/>
			</xsl:call-template>
			<xsl:call-template name="connector-start"/>
			<xsl:call-template name="atom"/>
		</Implies>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="fact">
		<xsl:param name="parent-ID"/>
		<xsl:call-template name="atom">
			<xsl:with-param name="parent-ID" select="$parent-ID"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="query">
		<Query>
			<xsl:call-template name="label"/>
			<xsl:call-template name="connector-start"/>
		</Query>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="integrity">
		<Protect>
			<Integrity>
				<xsl:call-template name="label"/>
				<xsl:call-template name="connector-start"/>
			</Integrity>
		</Protect>
	</xsl:template>
	
	<xsl:template match="vdx:Shape" mode="equivalent">
		<xsl:variable name="page-context" select="../.."/>
		<xsl:variable name="this-ID" select="@ID"/>
		<xsl:variable name="label" select="vdx:Prop[vdx:Label='Label']/vdx:Value/text()"/>

		<!-- select the first connected atom as the "main" member of this equivalent cluster -->
		<xsl:variable name="main-atom-ID" select="string($page-context/vdx:Connects/vdx:Connect[@FromSheet=$page-context/vdx:Connects/vdx:Connect[@ToSheet=$this-ID]/@FromSheet and @ToSheet!=$this-ID]/@ToSheet)"/>
		<!-- parse the shapes connected to the equivalent shape and that are different from the main one -->
		<xsl:for-each select="$page-context/vdx:Connects/vdx:Connect[@FromSheet=$page-context/vdx:Connects/vdx:Connect[@ToSheet=$this-ID]/@FromSheet and @ToSheet!=$this-ID and @ToSheet!=$main-atom-ID]">
			<xsl:variable name="end-ID" select="string(./@ToSheet)"/>
			<Equivalent>
				<xsl:if test="$label!=''">
					<oid>
						<Ind>
							<xsl:value-of select="$label"/>
						</Ind>
					</oid>
				</xsl:if>
				<xsl:apply-templates select="$page-context/vdx:Shapes/vdx:Shape[@ID=$main-atom-ID]"/>
				<xsl:apply-templates select="$page-context/vdx:Shapes/vdx:Shape[@ID=$end-ID]"/>
			</Equivalent>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:NafAtom']/@ID]">
		<Naf>
			<xsl:call-template name="atom"/>
		</Naf>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Atom']/@ID]">
		<xsl:call-template name="atom"/>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:And']/@ID]">
		<xsl:param name="parent-ID"/>
		<And>
			<xsl:call-template name="connector-start">
				<xsl:with-param name="parent-ID" select="$parent-ID"/>
			</xsl:call-template>
		</And>
	</xsl:template>
	
	<xsl:template match="vdx:Shape[@Master=/vdx:VisioDocument/vdx:Masters/vdx:Master[vdx:PageSheet/vdx:Misc/vdx:ShapeKeywords='RuleML:Or']/@ID]">
		<xsl:param name="parent-ID"/>
		<Or>
			<xsl:call-template name="connector-start">
				<xsl:with-param name="parent-ID" select="$parent-ID"/>
			</xsl:call-template>
		</Or>
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
		<Atom>
			<xsl:call-template name="label"/>
			<xsl:call-template name="parse-predicate">
				<xsl:with-param name="predicate-type">Rel</xsl:with-param>
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
		</Atom>
	</xsl:template>
	
	<xsl:template match="text()" mode="trim-string">
		<xsl:choose>
			<xsl:when test="$strict='true'">
				<xsl:value-of select="normalize-space(.)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="parse-predicate">
		<xsl:param name="predicate-type"/>
		<xsl:param name="predicate-string"/>
		
		<xsl:choose>
			<!-- support of sloted predicates -->
			<xsl:when test="$predicate-type='Ind' and starts-with($predicate-string, '(?')">
				<slot>
					<Ind><xsl:value-of select="substring-before(substring-after($predicate-string, '(?'), ')')"/></Ind>
					<xsl:call-template name="parse-predicate">
						<xsl:with-param name="predicate-type" select="$predicate-type"/>
						<xsl:with-param name="predicate-string" select="normalize-space(substring-after($predicate-string, ')'))"/>
					</xsl:call-template>
				</slot>
			</xsl:when>
			<!-- support of xs typed predicates -->
			<xsl:when test="$predicate-type='Ind' and starts-with($predicate-string, '(xs:')">
				<Data><xsl:attribute name="xsi:type"><xsl:value-of select="substring-before(substring-after($predicate-string, '('), ')')"/></xsl:attribute><xsl:value-of select="normalize-space(substring-after($predicate-string, ')'))"/></Data>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="{$predicate-type}">
					<xsl:choose>
						<xsl:when test="contains($predicate-string, '()')"><xsl:attribute name="uri">nxbre://binder</xsl:attribute><xsl:value-of select="normalize-space(substring-before($predicate-string, '()'))"/></xsl:when>
						<xsl:when test="starts-with($predicate-string, 'binder:')"><xsl:attribute name="uri">nxbre://binder</xsl:attribute><xsl:value-of select="normalize-space(substring-after($predicate-string, 'binder:'))"/></xsl:when>
						<xsl:when test="starts-with($predicate-string, 'expr:')"><xsl:attribute name="uri">nxbre://expression</xsl:attribute><xsl:value-of select="normalize-space(substring-after($predicate-string, 'expr:'))"/></xsl:when>
						<xsl:when test="starts-with($predicate-string, '>=')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>GreaterThanEqualTo(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '&lt;=')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>LessThanEqualTo(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '&lt;>') or starts-with($predicate-string, '!=')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>NotEquals(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '==')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>Equals(<xsl:value-of select="normalize-space(substring($predicate-string, 3))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '>')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>GreaterThan(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '&lt;')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>LessThan(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
						<xsl:when test="starts-with($predicate-string, '=')"><xsl:attribute name="uri">nxbre://operator</xsl:attribute>Equals(<xsl:value-of select="normalize-space(substring($predicate-string, 2))"/>)</xsl:when>
						<xsl:otherwise>
								<xsl:value-of select="$predicate-string"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:element>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="predicate">
		<xsl:param name="predicate-string"/>
		<xsl:choose>
			<xsl:when test="$strict='true'">
				<xsl:choose>
					<xsl:when test="starts-with($predicate-string, '?')">
						<Var>
							<xsl:value-of select="substring-after($predicate-string, '?')"/>
						</Var>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="parse-predicate">
							<xsl:with-param name="predicate-type">Ind</xsl:with-param>
							<xsl:with-param name="predicate-string" select="$predicate-string"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="starts-with($predicate-string, '?')">
						<Var>
							<xsl:value-of select="normalize-space(substring-after($predicate-string, '?'))"/>
						</Var>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="parse-predicate">
							<xsl:with-param name="predicate-type">Ind</xsl:with-param>
							<xsl:with-param name="predicate-string" select="normalize-space($predicate-string)"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="predicates">
		<xsl:param name="predicate-list"/>
		<xsl:choose>
			<xsl:when test="$strict='true'">
				<xsl:variable name="trim-predicate-list">
					<xsl:value-of select="normalize-space($predicate-list)"/>
				</xsl:variable>
				<xsl:if test="string-length($trim-predicate-list)">
					<xsl:choose>
						<xsl:when test="contains($trim-predicate-list, ',')">
							<xsl:call-template name="predicate">
								<xsl:with-param name="predicate-string" select="substring-before($trim-predicate-list, ',')"/>
							</xsl:call-template>
							<xsl:call-template name="predicates">
								<xsl:with-param name="predicate-list" select="substring-after($trim-predicate-list, ',')"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="predicate">
								<xsl:with-param name="predicate-string" select="$trim-predicate-list"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
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
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="label">
		<xsl:if test="vdx:Prop[vdx:Label='Label']">
			<oid>
				<Ind>
					<xsl:value-of select="vdx:Prop[vdx:Label='Label']/vdx:Value/text()"/>
				</Ind>
			</oid>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="implication.label">
		<xsl:param name="action"/>
			<oid>
				<Ind>
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
			</Ind>
		</oid>
	</xsl:template>
</xsl:stylesheet>
