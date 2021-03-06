<?xml version="1.0" encoding="UTF-16"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:var="http://schemas.microsoft.com/BizTalk/2003/var" exclude-result-prefixes="msxsl var s0" version="1.0" xmlns:ns0="http://BizWTF.Mocking.Schemas.Submission" xmlns:s0="http://BizWTF.Mocking.Services" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output omit-xml-declaration="yes" method="xml" version="1.0" />
  <xsl:template match="/">
    <xsl:apply-templates select="/s0:SubmitMessageResponse" />
  </xsl:template>
  <xsl:template match="/s0:SubmitMessageResponse">
    <ns0:MultipartMessage>
      <xsl:if test="s0:SubmitMessageResult/ns0:Description">
        <xsl:variable name="var:v1" select="string(s0:SubmitMessageResult/ns0:Description/@xsi:nil) = 'true'" />
        <xsl:if test="string($var:v1)='true'">
          <Description>
            <xsl:attribute name="xsi:nil">
              <xsl:value-of select="'true'" />
            </xsl:attribute>
          </Description>
        </xsl:if>
        <xsl:if test="string($var:v1)='false'">
          <Description>
            <xsl:value-of select="s0:SubmitMessageResult/ns0:Description/text()" />
          </Description>
        </xsl:if>
      </xsl:if>
      <PropertyBag>
        <xsl:for-each select="s0:SubmitMessageResult">
          <xsl:for-each select="ns0:PropertyBag">
            <xsl:for-each select="ns0:ContextProperty">
              <ns0:ContextProperty>
                <xsl:if test="ns0:Name">
                  <xsl:attribute name="Name">
                    <xsl:value-of select="ns0:Name/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:Namespace">
                  <xsl:attribute name="Namespace">
                    <xsl:value-of select="ns0:Namespace/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:Value">
                  <xsl:attribute name="Value">
                    <xsl:value-of select="ns0:Value/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:Promoted">
                  <xsl:attribute name="Promoted">
                    <xsl:value-of select="ns0:Promoted/text()" />
                  </xsl:attribute>
                </xsl:if>
              </ns0:ContextProperty>
            </xsl:for-each>
          </xsl:for-each>
        </xsl:for-each>
      </PropertyBag>
      <Parts>
        <xsl:for-each select="s0:SubmitMessageResult">
          <xsl:for-each select="ns0:Parts">
            <xsl:for-each select="ns0:Part">
              <ns0:Part>
                <xsl:if test="ns0:PartName">
                  <xsl:attribute name="PartName">
                    <xsl:value-of select="ns0:PartName/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:PartNumber">
                  <xsl:attribute name="PartNumber">
                    <xsl:value-of select="ns0:PartNumber/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:ContentType">
                  <xsl:attribute name="ContentType">
                    <xsl:value-of select="ns0:ContentType/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="ns0:IsBodyPart">
                  <xsl:attribute name="IsBodyPart">
                    <xsl:value-of select="ns0:IsBodyPart/text()" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:for-each select="ns0:Data">
                  <Data>
                    <xsl:copy-of select="./@*" />
                    <xsl:copy-of select="./*" />
                  </Data>
                </xsl:for-each>
                <xsl:if test="ns0:RawData">
                  <RawData>
                    <xsl:value-of select="ns0:RawData/text()" />
                  </RawData>
                </xsl:if>
              </ns0:Part>
            </xsl:for-each>
          </xsl:for-each>
        </xsl:for-each>
      </Parts>
    </ns0:MultipartMessage>
  </xsl:template>
</xsl:stylesheet>