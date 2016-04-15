<?xml version="1.0" encoding="UTF-8"?>
<!-- 

  NxBRE v2.0 - Pseudo-code rendering of xBusinessRules
  
  Author: David Dossot

-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	<xsl:strip-space elements="*"/>
	
	<xsl:param name="title">NxBRE - Pseudo-code rendering</xsl:param>

	<xsl:variable name="ucase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
	<xsl:variable name="lcase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
	
	<xsl:template match="/">
		<html>
			<head>
				<title><xsl:value-of select="$title"/></title>
				<style type="text/css">
					@media screen { a[href]:hover { background: #ffa } }
					font.standard { font-family: courier; color: black; font-size: 12px }
					font.constant { font-family: courier; font-weight: bold; color: black; font-size: 12px }
					font.type { font-family: courier; color: red; font-size: 12px }
					font.privatetype { font-family: courier; font-weight: bold; color: navy; background:Lavender ; font-size: 12px }
					font.keyword { font-family: courier; font-weight: bold; color: blue; font-size: 12px }
					font.keyword2 { font-family: courier; font-weight: bold; color: cadetblue; font-size: 12px }
					font.keyword3 { font-family: courier; color: maroon; font-size: 12px }
					font.id { font-family: courier; font-weight: bold; color: navy; font-size: 12px }
					font.comment { font-family: courier; font-style: italic; color: green; font-size: 12px }
					font.structure { font-family: courier; color: green; font-size: 12px }
					font.value { font-family: courier; color: navy; font-size: 12px }
					font.string { font-family: courier; color: magenta; font-size: 12px }
				</style>
			</head>
			<body bgcolor="white">
				<font class="standard">
					<a name="RULES_TOP"/>
					<xsl:apply-templates/>
					<a name="RULES_BOTTOM"/>
				</font>
			</body>
		</html>
	</xsl:template>
	
	<xsl:template match="Logic">
		<xsl:apply-templates/>
	</xsl:template>
	
	<xsl:template name="assignationvalue">
		<xsl:param name="type" select="name()"/>
		<xsl:param name="value" select="@value"/>
		<xsl:choose>
			<xsl:when test="$type='Date' or $type='DateTime' or $type='Time'">
				<font class="value">#<xsl:value-of select="$value"/>#</font>
			</xsl:when>
			<xsl:when test="$type='String'">
				<font class="string">"<xsl:value-of select="$value"/>"</font>
			</xsl:when>
			<xsl:when test="$type='Exception'">
				<font class="keyword2">new </font>
				<font class="type">exception</font>
				<font class="structure">(</font>
				<font class="string">"<xsl:value-of select="$value"/>"</font>
				<font class="structure">)</font>
			</xsl:when>
			
			<xsl:when test="$type='Assert' or $type='Modify'">
				<xsl:choose>
					<xsl:when test="@valueId">
						<xsl:value-of select="@valueId"/>
					</xsl:when>
					<xsl:when test="$value!=''">
						<font class="keyword2">new </font>
						<font class="type">
							<xsl:value-of select="translate(@type,$ucase,$lcase)"/>
						</font>
						<font class="structure">(</font>
						<xsl:call-template name="assignationvalue">
							<xsl:with-param name="type" select="@type"/>
							<xsl:with-param name="value" select="$value"/>
						</xsl:call-template>
						<font class="structure">)</font>
					</xsl:when>
					<xsl:when test="Argument">
						<font class="keyword2">new </font>
						<font class="privatetype">
							<xsl:value-of select="@type"/>
						</font>
						<font class="structure">(</font>
						<xsl:apply-templates select="Argument">
							<xsl:with-param name="indent" select="15+string-length(@id)+string-length(@type)"/>
						</xsl:apply-templates>
						<font class="structure">)</font>
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			
			<xsl:when test="$type='ObjectLookup'">
				<!-- only one of the 2 following should exist -->
				<xsl:value-of select="@type"/>
				<xsl:value-of select="@objectId"/>
				<xsl:text>.</xsl:text>
				<xsl:choose>
					<xsl:when test="string-length(@id)=0 or Argument">
						<font class="id">
							<xsl:value-of select="@member"/>
						</font>
						<font class="structure">(</font>
						<xsl:variable name="preindent">
							<xsl:choose>
								<xsl:when test="@id">
									<xsl:value-of select="10+string-length(@id)"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:apply-templates select="Argument">
							<xsl:with-param name="indent" select="2+$preindent+string-length(@type)+string-length(@objectId)+string-length(@member)"/>
						</xsl:apply-templates>
						<font class="structure">)</font>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@member"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			
			<xsl:otherwise>
				<font class="value">
					<xsl:value-of select="translate($value,$ucase,$lcase)"/>
				</font>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="assignation">
		<xsl:if test="@id">
			<font class="type">
				<xsl:choose>
					<xsl:when test="name()='Assert' or name()='ObjectLookup'">object</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="translate(name(),$ucase,$lcase)"/>
					</xsl:otherwise>
				</xsl:choose>
			</font>
			<xsl:text> </xsl:text>
			<xsl:value-of select="@id"/>
			<font class="structure"> = </font>
		</xsl:if>
		<xsl:call-template name="assignationvalue"/>
	</xsl:template>
	
	<xsl:template match="Integer | Short | Byte | Long | String | Exception | Double | Single | Decimal | Boolean | Date | DateTime | Time | ObjectLookup | Assert" mode="notassigned">
		<font class="structure">(</font>
		<xsl:call-template name="assignation"/>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="Integer | Short | Byte | Long | String | Exception | Double | Single | Decimal | Boolean | Date | DateTime | Time | ObjectLookup | Assert">
		<xsl:call-template name="assignation"/>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template name="constantboolean">
		<font class="keyword3">constant </font>
		<font class="type">boolean </font>
		<xsl:value-of select="@id"/>
		<font class="structure"> = </font>
		<xsl:call-template name="assignationvalue">
			<xsl:with-param name="type">Boolean</xsl:with-param>
			<xsl:with-param name="value" select="translate(name(),$ucase,$lcase)"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="True | False" mode="notassigned">
		<font class="structure">(</font>
		<xsl:call-template name="constantboolean"/>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="True | False">
		<xsl:call-template name="constantboolean"/>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template name="modify">
		<xsl:value-of select="@id"/>
		<xsl:text> = </xsl:text>
		<xsl:call-template name="assignationvalue"/>
	</xsl:template>
	
	<xsl:template match="Modify">
		<xsl:call-template name="modify"/>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="Modify" mode="notassigned">
		<font class="structure">(</font>
		<xsl:call-template name="modify"/>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template name="evaluate">
		<font class="keyword2">evaluate</font>
		<font class="structure">(</font>
		<font class="id">
			<xsl:value-of select="@id"/>
		</font>
		<xsl:if test="Parameter">
			<font class="structure">(</font>
			<xsl:apply-templates select="Parameter">
				<xsl:with-param name="indent" select="10+string-length(@id)"/>
			</xsl:apply-templates>
			<font class="structure">)</font>
		</xsl:if>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="Evaluate">
		<xsl:call-template name="evaluate"/>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="Evaluate" mode="notassigned">
		<font class="structure">(</font>
		<xsl:call-template name="evaluate"/>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="Retract">
		<font class="id">
			<xsl:value-of select="@id"/>
		</font>
		<font class="structure"> = </font>
		<font class="constant">null</font>
		<font class="structure">;</font>
		<br/>
	</xsl:template>

	<xsl:template name="doindent">
		<xsl:param name="indent"/>
		<xsl:if test="$indent>0">
			<xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
			<xsl:call-template name="doindent">
				<xsl:with-param name="indent" select="-1+$indent"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Argument">
		<xsl:param name="indent"/>
		<xsl:if test="position()>1">
			<font class="structure">,<br/>
				<xsl:call-template name="doindent">
					<xsl:with-param name="indent" select="$indent"/>
				</xsl:call-template>
			</font>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="@valueId">
				<xsl:value-of select="@valueId"/>
			</xsl:when>
			<xsl:when test="@value">
				<xsl:choose>
					<xsl:when test="@type='String' or string-length(@type)=0">
						<font class="string">"<xsl:value-of select="@value"/>"</font>
					</xsl:when>
					<xsl:when test="@type='Date' or @type='DateTime' or @type='Time'">
						<font class="value">#<xsl:value-of select="@value"/>#</font>
					</xsl:when>
					<xsl:otherwise>
						<font class="value">
							<xsl:value-of select="@value"/>
						</font>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Parameter">
		<xsl:param name="indent"/>
		<xsl:if test="position()>1">
			<font class="structure">,<br/>
				<xsl:call-template name="doindent">
					<xsl:with-param name="indent" select="2+$indent"/>
				</xsl:call-template>
			</font>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<font class="structure"> = </font>
		<xsl:choose>
			<xsl:when test="@valueId">
				<xsl:value-of select="@valueId"/>
			</xsl:when>
			<xsl:when test="@value">
				<font class="string">"<xsl:value-of select="@value"/>"</font>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="increment">
		<xsl:value-of select="@id"/>
		<xsl:choose>
			<xsl:when test="@step">
				<xsl:choose>
					<xsl:when test="@step='1'">
						<font class="structure">++</font>
					</xsl:when>
					<xsl:otherwise>
						<font class="structure"> += </font>
						<font class="value">
							<xsl:value-of select="@step"/>
						</font>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="@valueId">
				<font class="structure"> = </font>
				<xsl:value-of select="@valueId"/>
			</xsl:when>
			<xsl:when test="@value">
				<font class="structure"> = </font>
				<font class="value">
					<xsl:value-of select="@value"/>
				</font>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Increment" mode="notassigned">
		<font class="structure">(</font>
		<xsl:call-template name="increment"/>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="Increment">
		<xsl:call-template name="increment"/>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="ForEach">
		<br/>
		<font class="keyword">foreach</font>
		<font class="structure">(</font>
		<font class="id">
			<xsl:value-of select="@id"/>
		</font>
		<font class="keyword"> in </font>
		<font class="id">
			<xsl:value-of select="@valueId"/>
		</font>
		<font class="structure">)</font>
		<br/>
		<font class="structure">{</font>
		<ul>
			<xsl:apply-templates/>
		</ul>
		<font class="structure">}</font>
		<br/>
		<br/>
	</xsl:template>
	
	<xsl:template match="If | ElseIf | While">
		<br/>
		<xsl:apply-templates select="And//comment() | Or//comment() | Not//comment()" mode="display"/>
		<font class="keyword">
			<xsl:value-of select="translate(name(),$ucase,$lcase)"/>
			<xsl:text> </xsl:text>
		</font>
		<xsl:if test="count(And/*)>1 or count(Or/*)>1">
			<font class="structure">(</font>
		</xsl:if>
		<xsl:apply-templates select="And | Or | Not"/>
		<xsl:if test="count(And/*)>1 or count(Or/*)>1">
			<font class="structure">)</font>
		</xsl:if>
		<br/>
		<font class="structure">{</font>
		<ul>
			<xsl:apply-templates select="Do"/>
		</ul>
		<font class="structure">}</font>
		<xsl:if test="not(following-sibling::ElseIf or following-sibling::Else)">
			<br/>
			<br/>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Else">
		<br/>
		<font class="keyword">else</font>
		<br/>
		<font class="structure">{</font>
		<ul>
			<xsl:apply-templates/>
		</ul>
		<font class="structure">}</font>
		<br/>
		<br/>
	</xsl:template>
	
	<xsl:template name="comparator">
		<xsl:param name="valueId" select="@valueId"/>
		<xsl:choose>
			<xsl:when test="*[@id=$valueId]">
				<xsl:apply-templates select="*[@id=$valueId]" mode="notassigned"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$valueId"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="logical">
		<xsl:if test="preceding-sibling::*">
			<font class="structure">
				<br/>
				<xsl:variable name="parent" select="name(..)"/>
				<xsl:choose>
					<xsl:when test="$parent='And'">and </xsl:when>
					<xsl:when test="$parent='Or'">or </xsl:when>
				</xsl:choose>
			</font>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Not">
		<xsl:call-template name="logical"/>
		<font class="structure">not </font>
		<xsl:apply-templates/>
	</xsl:template>
	
	<xsl:template name="operator">
		<xsl:param name="symbol"/>
		<xsl:param name="style">structure</xsl:param>
		<xsl:call-template name="logical"/>
		<font class="structure">(</font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@leftId"/>
		</xsl:call-template>
		<font class="{$style}">
			<xsl:text> </xsl:text>
			<xsl:value-of select="$symbol"/>
			<xsl:text> </xsl:text>
		</font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@rightId"/>
		</xsl:call-template>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="IsAsserted">
		<xsl:call-template name="logical"/>
		<font class="structure">(</font>
		<font class="id">
			<xsl:value-of select="@valueId"/>
		</font>
		<font class="structure"> != </font>
		<font class="constant">null</font>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="IsTrue">
		<xsl:call-template name="logical"/>
		<font class="structure">(</font>
		<xsl:call-template name="comparator"/>
		<font class="structure">)</font>
	</xsl:template>
	<xsl:template match="IsFalse">
		<xsl:call-template name="logical"/>
		<font class="structure">(not (</font>
		<xsl:call-template name="comparator"/>
		<font class="structure">))</font>
	</xsl:template>
	
	<xsl:template match="Between">
		<xsl:call-template name="logical"/>
		<font class="structure">(</font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@valueId"/>
		</xsl:call-template>
		<font class="structure">
			<xsl:choose>
				<xsl:when test="@excludeLeft='true'"> between ]</xsl:when>
				<xsl:otherwise> between [</xsl:otherwise>
			</xsl:choose>
		</font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@leftId"/>
		</xsl:call-template>
		<font class="structure"> and </font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@rightId"/>
		</xsl:call-template>
		<font class="structure">
			<xsl:choose>
				<xsl:when test="@excludeRight='true'">[ )</xsl:when>
				<xsl:otherwise>] )</xsl:otherwise>
			</xsl:choose>
		</font>
	</xsl:template>
	
	<xsl:template name="parsein">
		<xsl:param name="idList"/>
		<xsl:choose>
			<xsl:when test="contains($idList,',')">
				<xsl:call-template name="comparator">
					<xsl:with-param name="valueId" select="substring-before($idList,',')"/>
				</xsl:call-template>
				<font class="structure">, </font>
				<xsl:call-template name="parsein">
					<xsl:with-param name="idList" select="substring-after($idList,',')"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="comparator">
					<xsl:with-param name="valueId" select="$idList"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="In">
		<xsl:call-template name="logical"/>
		<font class="structure">(</font>
		<xsl:call-template name="comparator">
			<xsl:with-param name="valueId" select="@valueId"/>
		</xsl:call-template>
		<font class="structure"> in (</font>
		<xsl:call-template name="parsein">
			<xsl:with-param name="idList" select="@idList"/>
		</xsl:call-template>
		<font class="structure">)</font>
	</xsl:template>
	
	<xsl:template match="Equals">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">==</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="NotEquals">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">!=</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="LessThanEqualTo">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">&lt;=</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="LessThan">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">&lt;</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="GreaterThanEqualTo">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">&gt;=</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="GreaterThan">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">&gt;</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="InstanceOf">
		<xsl:call-template name="operator">
			<xsl:with-param name="symbol">instance of</xsl:with-param>
			<xsl:with-param name="style">keyword2</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="Set">
		<a name="SET_{@id}">
			<font class="keyword">set </font>
			<font class="id">
				<xsl:value-of select="@id"/>
			</font>
		</a>
		<font class="structure"> {</font>
		<ul>
			<xsl:apply-templates/>
		</ul>
		<font class="structure"> }</font>
		<br/>
		<br/>
	</xsl:template>
	
	<xsl:template match="InvokeSet">
		<font class="keyword2">invoke set</font>
		<font class="structure">(</font>
		<xsl:if test="@valueId">
			<xsl:value-of select="@valueId"/>
		</xsl:if>
		<xsl:if test="@id">
			<font class="id">
				<a href="#SET_{@id}">
					<xsl:value-of select="@id"/>
				</a>
			</font>
		</xsl:if>
		<font class="structure">);</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="comment()" name="comment">
		<font class="comment">
			<xsl:text>/* </xsl:text>
			<br/>
			<xsl:value-of select="."/>
			<br/>
			<xsl:text> */</xsl:text>
		</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="And//comment() | Or//comment() | Not//comment()"/>
	
	<xsl:template match="And//comment() | Or//comment() | Not//comment()" mode="display">
		<xsl:call-template name="comment"/>
	</xsl:template>
	
	<xsl:template name="newexceptionbase">
		<font class="keyword2"> new </font>
		<font class="id">
			<xsl:value-of select="concat('BRE', substring(name(),6))"/>
		</font>
		<xsl:text>(</xsl:text>
		<xsl:if test="@valueId">
			<xsl:value-of select="@valueId"/>
		</xsl:if>
		<xsl:if test="@value">
			<font class="string">"<xsl:value-of select="@value"/>"</font>
		</xsl:if>
		<xsl:text>)</xsl:text>
	</xsl:template>
	
	<xsl:template match="ThrowException | ThrowFatalException">
		<xsl:param name="exceptiontype"/>
		<xsl:choose>
			<xsl:when test="@id">
				<font class="id">
					<xsl:value-of select="concat('BRE', substring(name(),6))"/>
				</font>
				<xsl:text> </xsl:text>
				<xsl:value-of select="@id"/>
				<xsl:text> = </xsl:text>
				<xsl:call-template name="newexceptionbase"/>
				<font class="structure">;</font>
				<br/>
				<font class="keyword2">throw </font>
				<xsl:value-of select="@id"/>
			</xsl:when>
			<xsl:otherwise>
				<font class="keyword2">throw </font>
				<xsl:call-template name="newexceptionbase"/>
			</xsl:otherwise>
		</xsl:choose>
		<font class="structure">;</font>
		<br/>
	</xsl:template>
	
	<xsl:template match="Log">
		<font class="keyword2">log</font>
		<font class="structure">(</font>
		<font class="value">
			<xsl:value-of select="@level"/>
		</font>
		<font class="structure">, </font>
		<xsl:if test="@msgId">
			<xsl:value-of select="@msgId"/>
		</xsl:if>
		<xsl:if test="@msg">
			<font class="string">"<xsl:value-of select="@msg"/>"</font>
		</xsl:if>
		<font class="structure">);</font>
		<br/>
	</xsl:template>

</xsl:stylesheet>
