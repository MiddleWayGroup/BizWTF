<?xml version="1.0" encoding="UTF-16"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:var="http://schemas.microsoft.com/BizTalk/2003/var" exclude-result-prefixes="msxsl var" version="1.0" xmlns:ns1="http://BizWTF.Mocking.Schemas.Submission" xmlns:ns0="http://BizWTF.Mocking.Services" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output omit-xml-declaration="yes" method="xml" version="1.0" />
  <xsl:template match="/">
    <xsl:apply-templates select="/ns1:MultipartMessage" />
  </xsl:template>
  <xsl:template match="/ns1:MultipartMessage">
    <ns0:SubmitMessage>
      <ns0:inputMessage>
        <xsl:if test="Description">
          <xsl:variable name="var:v1" select="string(Description/@xsi:nil) = 'true'" />
          <xsl:if test="string($var:v1)='true'">
            <ns1:Description>
              <xsl:attribute name="xsi:nil">
                <xsl:value-of select="'true'" />
              </xsl:attribute>
            </ns1:Description>
          </xsl:if>
          <xsl:if test="string($var:v1)='false'">
            <ns1:Description>
              <xsl:value-of select="Description/text()" />
            </ns1:Description>
          </xsl:if>
        </xsl:if>
        <ns1:Parts>
          <xsl:for-each select="Parts/ns1:Part">
            <ns1:Part>
              <xsl:if test="@ContentType">
                <ns1:ContentType>
                  <xsl:value-of select="@ContentType" />
                </ns1:ContentType>
              </xsl:if>
              <xsl:for-each select="Data">
                <ns1:Data>
                  <xsl:copy-of select="./@*" />
                  <xsl:copy-of select="./*" />
                </ns1:Data>
              </xsl:for-each>
              <ns1:IsBodyPart>
                <xsl:value-of select="@IsBodyPart" />
              </ns1:IsBodyPart>
              <xsl:if test="@PartName">
                <ns1:PartName>
                  <xsl:value-of select="@PartName" />
                </ns1:PartName>
              </xsl:if>
              <xsl:if test="@PartNumber">
                <ns1:PartNumber>
                  <xsl:value-of select="@PartNumber" />
                </ns1:PartNumber>
              </xsl:if>
              <ns1:RawData>
                <xsl:value-of select="RawData/text()" />
              </ns1:RawData>
            </ns1:Part>
          </xsl:for-each>
        </ns1:Parts>
        <ns1:PropertyBag>
          <xsl:for-each select="PropertyBag/ns1:ContextProperty">
            <ns1:ContextProperty>
              <xsl:if test="@Name">
                <ns1:Name>
                  <xsl:value-of select="@Name" />
                </ns1:Name>
              </xsl:if>
              <xsl:if test="@Namespace">
                <ns1:Namespace>
                  <xsl:value-of select="@Namespace" />
                </ns1:Namespace>
              </xsl:if>
              <ns1:Promoted>
                <xsl:value-of select="@Promoted" />
              </ns1:Promoted>
              <xsl:if test="@Value">
                <ns1:Value>
                  <xsl:value-of select="@Value" />
                </ns1:Value>
              </xsl:if>
            </ns1:ContextProperty>
          </xsl:for-each>
        </ns1:PropertyBag>
      </ns0:inputMessage>
    </ns0:SubmitMessage>
  </xsl:template>
</xsl:stylesheet>