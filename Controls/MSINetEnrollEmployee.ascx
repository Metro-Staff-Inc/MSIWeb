<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetEnrollEmployee.ascx.cs" Inherits="Controls_MSINetEnrollEmployee" %>
<style type="text/css">
	html, body {
		height: 100%;
		overflow: auto;
	}
	body {
		padding: 0;
		margin: 0;
	}
	#silverlightControlHost {
		height: 100%;
		text-align:center;
	}
	.style1
	{
		width: 100%;
	}
	.style2
	{
		width: 176px;
		font-size: medium;
	}
	.style3
	{
		width: 176px;
		font-size: medium;
		height: 32px;
	}
	.style4
	{
		height: 32px;
	}
	</style>
	<script type="text/javascript" src="..\\Silverlight.js"></script>
	<script type="text/javascript">
		function onSilverlightError(sender, args) {
			var appSource = "";
			if (sender != null && sender != 0) {
				appSource = sender.getHost().Source;
			}

			var errorType = args.ErrorType;
			var iErrorCode = args.ErrorCode;

			if (errorType == "ImageError" || errorType == "MediaError") {
				return;
			}

			var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

			errMsg += "Code: " + iErrorCode + "    \n";
			errMsg += "Category: " + errorType + "       \n";
			errMsg += "Message: " + args.ErrorMessage + "     \n";

			if (errorType == "ParserError") {
				errMsg += "File: " + args.xamlFile + "     \n";
				errMsg += "Line: " + args.lineNumber + "     \n";
				errMsg += "Position: " + args.charPosition + "     \n";
			}
			else if (errorType == "RuntimeError") {
				if (args.lineNumber != 0) {
					errMsg += "Line: " + args.lineNumber + "     \n";
					errMsg += "Position: " + args.charPosition + "     \n";
				}
				errMsg += "MethodName: " + args.methodName + "     \n";
			}
			alert(errMsg);
			throw new Error(errMsg);
		}
	</script>

<asp:Wizard ID="Wizard1" runat="server" BackColor="#EFF3FB" 
	BorderColor="#B5C7DE" BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" 
	Height="440px" Width="720px" ActiveStepIndex="0" >
	<HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" 
		BorderWidth="2px" Font-Bold="True" Font-Size="0.9em" ForeColor="White" 
		HorizontalAlign="Center" />
	<NavigationButtonStyle BackColor="White" BorderColor="#507CD1" 
		BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
		ForeColor="#284E98" />
	<SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" 
		ForeColor="White" />
	<SideBarStyle BackColor="#507CD1" BorderWidth="1" BorderStyle="Solid" Font-Size="0.9em" 
		VerticalAlign="Top" Width="150px" />
	<StepStyle Font-Size="0.8em" ForeColor="#333333" VerticalAlign="Top" />
	<WizardSteps>
		<asp:WizardStep ID="WizardStep1" runat="server" title="Employee Info">
			<table style="font-size:medium; height: 120px;" cellpadding="4" class="style1">
				<tr>
					<td class="style2" >
						First Name:</td>
					<td>
						<asp:TextBox ID="TextBox1" runat="server" Width="183px"></asp:TextBox>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td class="style2">
						Last Name:</td>
					<td>
						<asp:TextBox ID="TextBox2" runat="server" Width="183px"></asp:TextBox>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td class="style3">
						Street Address:</td>
					<td class="style4">
						<asp:TextBox ID="TextBox3" runat="server" Width="183px"></asp:TextBox>
					</td>
					<td class="style4">
						</td>
				</tr>
				<tr>
					<td class="style2">
						City, State:</td>
					<td>
						<asp:TextBox ID="TextBox4" runat="server" Width="183px"></asp:TextBox>
						<asp:DropDownList ID="ddlState" runat="server" >
						</asp:DropDownList>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td class="style2">
						Zip Code:</td>
					<td>
						<asp:TextBox ID="TextBox5" runat="server" Width="183px"></asp:TextBox>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td class="style2">
						Phone:</td>
					<td>
						<asp:TextBox ID="TextBox6" runat="server" Width="183px"></asp:TextBox>
					</td>
					<td>
						&nbsp;</td>
				</tr>
			</table>
		</asp:WizardStep>
		<asp:WizardStep ID="WizardStep2" runat="server" Title="Skill Set">
			<table cellpadding="4" class="style1">
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
					<td>
						&nbsp;</td>
				</tr>
			</table>
		</asp:WizardStep>
		<asp:WizardStep ID="WizardStep3" runat="server" Title="Fingerprint">
<object id="DPFPCapture" classid="CLSID:3FA859DA-300C-4247-BD33-6011198399C0">
</object>

<comment>
	<embed type="application/x-eskerplus" id="DPFPCapture" classid="CLSID:3FA859DA-300C-4247-BD33-6011198399C0">
	 </embed>
</comment>

<object id="DPFPFeatureExtraction" classid="CLSID:C64055AD-8960-4429-BDB4-2E102F47BD9A"></object>

<COMMENT>
	<EMBED type="application/x-eskerplus" id="DPFPFeatureExtraction" classid="CLSID:C64055AD-8960-4429-BDB4-2E102F47BD9A">
	 </EMBED>
</COMMENT>

<OBJECT ID="DPFPEnrollment" CLASSID="CLSID:1E1020EF-4A4F-430D-A351-427821B177B2"></OBJECT>

<COMMENT>
	<EMBED type="application/x-eskerplus" id="DPFPEnrollment" classid="CLSID:1E1020EF-4A4F-430D-A351-427821B177B2">
	 </EMBED>
</COMMENT>

<script language="VBScript">
 
   Sub DPFPCapture_OnComplete(SerNum, Sample)
	 Dim Quality
	 Dim Ftrs
	 Dim Tmpl
	 Dim b
	 SampleCapture.value = "The image was acquired from reader " & SerNum & "."
	 Quality = DPFPFeatureExtraction.CreateFeatureSet(Sample, 2)
	 ImgQuality.value = Quality
	 Set Ftrs = DPFPFeatureExtraction.FeatureSet
	 b = Ftrs.Serialize()
	 SampleBytesText.value = Lenb(b)
	 DPFPEnrollment.AddFeatures(Ftrs)
	 FeaturesNeededText.value = DPFPEnrollment.FeaturesNeeded
	 TemplateStatusText.value = DPFPEnrollment.TemplateStatus
	 
	 if DPFPEnrollment.TemplateStatus = 3 Then
	   Set Tmpl = DPFPEnrollment.Template
	   bTmpl = Tmpl.Serialize()
	   TmplBytesText.value = Lenb(bTmpl)
	 End if
	 
   End Sub
   
   Sub DPFPCapture_OnReaderConnect(SerNum)
	 SampleCapture.value = "Fingerprint reader " & SerNum & " was connected."
   End Sub
   
   Sub DPFPCapture_OnReaderDisconnect(SerNum)
	 SampleCapture.value = "Fingerprint reader " & SerNum & " was disconnected."
   End Sub
   
   Sub DPFPCapture_OnFingerTouch(SerNum)
	 SampleCapture.value = "Finger coming from reader " & SerNum & "."
   End Sub
   
   Sub DPFPCapture_OnFingerGone(SerNum)
	 SampleCapture.value = "Finger gone from reader " & SerNum &"."
   End Sub
   
   Sub DPFPCapture_OnSampleQuality(SerNum, CaptureFeedback)
	 SampleCapture.value = "Finger sample quality from reader " & SerNum &"."
   End Sub   
	 
   Sub StartCapture()
	SampleCapture.value = "Capture started!"
	DPFPCapture.Priority = 2
	DPFPCapture.StartCapture()
   End Sub
   
   Sub StopCapture()
	DPFPCapture.StopCapture()
	SampleCapture.value = "Capture stopped!"
   End Sub

</script>
		</asp:WizardStep>
		<asp:WizardStep ID="WizardStep4" runat="server" Title="Photo">
			<div id="silverlightControlHost">
			<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="540px" height="480px">
			<%
				string orgSourceValue = @"../ClientBin/SilverlightWebcam.xap";
				string param;

				if (System.Diagnostics.Debugger.IsAttached)
					param = "<param name=\"source\" value=\"" + orgSourceValue + "?ignore=Debugger\" />";
				else
				{
					string xappath = HttpContext.Current.Server.MapPath(@"") + @"\" + orgSourceValue;
					DateTime xapCreationDate = System.IO.File.GetLastWriteTime(xappath);
					param = "<param name=\"source\" value=\"" + orgSourceValue + "?ignore=" + xapCreationDate.ToString() + "\" />";
				}
				Response.Write(param);
			%>
			<param name="initParams" value="<%= string.Format("WCFReferenceURL={0},key1={1},key2={2}", "someValue1", "someValue1", "someValue2")%>" />
			<param name="initParams" value="<%= string.Format("picName={0}", GetEnteredEmployeeID())%>" />
			<param name="onError" value="onSilverlightError" />
			<param name="background" value="blue" />
			<param name="minRuntimeVersion" value="4.0.50826.0" />
			<param name="autoUpgrade" value="true" />
			<a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration:none">
			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
			</a>
			</object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
		</div>
		</asp:WizardStep>
		<asp:WizardStep ID="WizardStep5" runat="server" StepType="Finish" Title="Finish">
		</asp:WizardStep>
		<asp:WizardStep ID="WizardStep6" runat="server" StepType="Complete" Title="Complete">
		</asp:WizardStep>
	</WizardSteps>
</asp:Wizard>

