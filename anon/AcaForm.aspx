<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcaForm.aspx.cs" Inherits="anon_AcaForm" %>

<!doctype html>
<html lang="en">
  <head runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css" 
        integrity="sha384-TX8t27EcRE3e/ihU7zmQxVncDAy5uIKz4rEkgIXeMed4M0jlfIDPvg6uqKI2xXr2" crossorigin="anonymous">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">    
      <link href="../Styles/ACAForm.css" rel="stylesheet" />
    <title>ACA Acceptance Form</title>
</head>
  <body>

<div id="div_main" class="container"> <!-- main container -->
    <h1>ACA 2018-2019 Enrollment Form</h1>
    <br />

    <!-- login section - provide id and last 4 of ssn -->
    <div id="div_login" class="tab container-fluid login" style="display:none">
        <div class="row mt-2">
            <div class="col-sm-3">
                <label for="metro_staff_id">Metro Staff ID#:</label>
            </div>
            <div class="col-sm-3">
                <input name="metro_staff_id" placeholder="Metro Staff ID..." id="metro_staff_id" oninput="this.className = ''"><br />
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-3">
                <label for="last_four">Last 4 Digits of Social Security #:</label>
            </div>
            <div class="col-sm-3">
                <input name="last_four" placeholder="Last 4 digits of Social Security #..." id="ssn_last_four" oninput="this.className = ''"><br />
            </div>
        </div>
    </div>

    <!-- employee_info section - display employee info and allow updates to some fields -->
    <div id="div_employee_info" class= "tab container-fluid py-2 collapse employee_info">
        <div class="row mt-2">
            <div class="col-sm-3 text-left">
                <p id="ei_label_last_name" class="font-weight-bolder">Last Name</p>
                <p id="ei_last_name"></p>
            </div>
            <div class="col-sm-3 text-left">
                <p id="ei_label_first_name" class="font-weight-bolder">First Name</p"font-weight-bolder">
                <p id="ei_first_name"></p>
            </div>
            <div class="col-sm-2 text-left">
                <p id="ei_label_middle_initial" class="font-weight-bolder">Mid</p>
                <p id="ei_middle_initial"></p>
            </div>
            <div class="col-sm-4 text-left">
                <p id="ei_label_social_security" class="font-weight-bolder">Social Security</p>
                <p id="ei_ssn"></p>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-3 text-left">
                <p id="ei_label_mailing_address" class="font-weight-bolder">Address</p>
                <input type="text" id="ei_address" onchange="activateUpdateButton()"/>
            </div>
            <div class="col-sm-3 text-left">
                <p id="ei_label_city" class="font-weight-bolder">City</p>
                <input type="text" id="ei_city" onchange="activateUpdateButton()"/>
            </div>
            <div class="col-sm-2 text-left">
                <p id="ei_label_state" class="font-weight-bolder">State</p>
                <input type="text" size="6" id="ei_state" onchange="activateUpdateButton()" />
            </div>
            <div class="col-sm-2 text-left">
                <p id="ei_label_zip" class="font-weight-bolder">Zip Code</p>
                <input type="text" size="6" id="ei_zip" onchange="activateUpdateButton()" />
            </div>
        </div> 
        <div class="row mt-2">
            <div class="col-sm-3 text-left">
                <p id="ei_label_email" class="font-weight-bolder">Email</p>
                <input type="text" id="ei_email" onchange="activateUpdateButton()"/>
            </div>
            <div class="col-sm-3 text-left">
                <p id="ei_label_phone" class="font-weight-bolder">Phone</p>
                <input type="text" id="ei_phone" onchange="activateUpdateButton()" />
            </div>
            <div class="col-sm-2 text-left">
                <p class="font-weight-bolder">DOB</p>
                <input size="6" type="text" id="ei_date_of_birth" onchange="activateUpdateButton()"/>
            </div>
            <div class="col-sm-4 text-left">
                <p class="font-weight-bolder">Metro Staff Inc.</p>
                <p>Elgin, Il 60123</p>
                <p>(847)742-9900</p>
            </div>
        </div>
        <div class="row mt-2">
            <div class="col-sm-4 text-left">
                <label class="checkbox-inline">
                    <span id="ei_gender" style="padding-right:8px">Gender: </span>
                </label>
                <label class="checkbox-inline">
                    <input type="radio" name="inp_gender" value="M" onchange="activateUpdateButton()"/><span style="padding-left:2px; padding-right:12px" id="ei_gender_male">Male</span>
                </label>
                <label class="checkbox-inline">
                    <input type="radio" name="inp_gender" value="F" onchange="activateUpdateButton()"/><span style="padding-left:2px" id="ei_gender_female">Female</span>
                </label>
            </div>
            <div class="col-sm-4 text-left">
                <label id="ei_marital_status" for="marital_status">Marital Status:</label>
                <select id="marital_status" name="marital_status" onchange="activateUpdateButton()">
                    <option id="ei_option_single" value="single">Single</option>
                    <option id="ei_option_married" value="married">Married</option>
                    <option id="ei_option_divorced" value="divorced">Divorced</option>
                    <option id="ei_legally_separated" value="legally_separated">Legally Separated</option>
                    <option id="ei_widowed" value="widowed">Widowed</option>
                </select>
            </div>
        </div>
        <input type="button" id="btnUpdateEmployeeInfo" value="Update Employee Info" disabled="disabled" onclick="updateEmployeeInfo()"/>
    </div>

    <!--<h3 style="display:none" id="plan_selection"></h3>-->

    <!-- plan_selection section - choose new plan or update existing -->
    <div id="div_plan_selection" class="container tab">
        <hr />
        <div class="col">
            <span>Benefit Plan Selection Information for:</span>
        </div>
        <div class="col">
            <span id="coverage_type">New Coverage</span>
        </div>
    </div>

    <div id="div_coverage_options" class="container tab">
        <hr />
        <div class="row mt-2">
            <div class="col-sm-3 text-left">
                <p class="font-weight-bolder">Value Plan (MEC PLUS)</p>
            </div>
            <div class="col-sm-3 text-left">
                <p style="text-align-last:right" class="font-weight-bolder">Weekly Cost</p>
            </div>
            <div class="col-sm-3 text-left">
                <p class="font-weight-bolder">MVP Bronze</p>
            </div>
            <div class="col-sm-3 text-left">
                <p style="text-align-last:right" class="font-weight-bolder">Monthly Premium</p>
            </div>
        </div>
        <div class="row mt-2" style="border:solid thin black">
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="employee_only2" name="plan" value="employee_only">
                <label style="padding:10px" for="employee_only">Employee Only</label><br>
            </div>
            <div class="col-sm-3 text-left" ; style="border-right:solid thin black">
                <p style="text-align-last:right" class="font-weight-bolder">$11.54</p>
            </div>
            <div class="col-sm-3 text-left" >
                <input style="padding:10px"  type="radio" id="mvp_employee_only" name="plan" value="mvp_employee_only" disabled="disabled">
                <label style="padding:10px" for="mvp_employee_only">MVP Employee Only</label><br>
            </div>
            <div class="col-sm-3 text-left" >
                <p style="text-align:right" class="font-weight-bolder">$547.50</p>
            </div>
        </div>
        <div class="row mt-2" style="border:solid thin black">
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="employee_and_spouse2" name="plan" value="employee_and_spouse">
                <label style="padding:10px" for="employee_and_spouse">Employee And Spouse</label><br>
            </div>
            <div class="col-sm-3" style="border-right:solid thin black">
                <p style="text-align-last:right" class="font-weight-bolder">$31.43</p>
            </div>
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="mvp_employee_and_spouse" name="plan" value="mvp_employee_and_spouse" disabled="disabled">
                <label style="padding:10px" for="mvp_employee_and_spouse">MVP Employee And Spouse</label><br>
            </div>
            <div class="col-sm-3">
                <p style="text-align:right" class="font-weight-bolder">$1,085.40</p>
            </div>
        </div>
        <div class="row mt-2" style="border:solid thin black">
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="employee_and_children2" name="plan" value="employee_and_children">
                <label style="padding:10px" for="employee_and_children">Employee And Children</label><br>
            </div>
            <div class="col-sm-3 text-left" style="border-right:solid thin black">
                <p style="text-align-last:right" class="font-weight-bolder">$30.34</p>
            </div>
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="mvp_employee_and_children" name="plan" value="mvp_employee_and_children" disabled="disabled">
                <label style="padding:10px" for="mvp_employee_and_children">MVP Employee & Children</label><br>
            </div>
            <div class="col-sm-3 text-left">
                <p style="text-align:right" class="font-weight-bolder">$963.15</p>
            </div>
        </div>
        <div class="row mt-2" style="border:solid thin black">
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="employee_and_family2" name="plan" value="employee_and_family">
                <label style="padding:10px" for="employee_and_family">Employee And Family</label><br>
            </div>
            <div class="col-sm-3 text-left" style="border-right:solid thin black">
                <p style="text-align-last:right" class="font-weight-bolder">$56.80</p>
            </div>
            <div class="col-sm-3 text-left">
                <input style="padding:10px" type="radio" id="mvp_employee_and_family" name="plan" value="mvp_employee_and_family" disabled="disabled">
                <label style="padding:10px" for="mvp_employee_and_family">MVP Employee And Family</label><br>
            </div>
            <div class="col-sm-3 text-left">
                <p style="text-align:right" class="font-weight-bolder">$1,501.05</p>
            </div>
        </div>

        <div class="row mt-2">
            <div class="col-sm-12 text-left">
                <span>
                    For MVP Bronze Plan - If electing this product, you must call the enrollment office at 888-232-9431 and answer the following question:
                </span>
            </div>
            <div class="col-sm-8" style="border:thin solid black">
                <span>
                    Have you or any of your dependents electing the MVP plan incurred more than $10,000 in medical expenses in the past 12 months, been adviseds to be hospitalized, undergone surgery, advised to be tested to determine if you have cancer, or are currently pregnant?
                </span>
            </div>
            <div class="col-sm-4" style="border:thin solid black">
                <span>
                    <input type="radio" id="bronze_yes" name="coverage" value="bronze_yes">
                    <label for="bronze_yes">Yes</label><br>
                    <input type="radio" id="bronze_no" name="coverage" value="bronze_no">
                    <label for="bronze_no">No</label><br>
                </span>
            </div>
        </div>
        <div class="row mt-2">
            <div>
                <span>
                    If "Yes" has been checked, please fill out the medical information page attached to the 
                    enrollment application. <b>"MVP Bronze rates are illustrative and are subject to change based
                    on the single medical questionnaire which is determined by medical underwriting.  Employee
                    premium for the MVP Bronze plan is not to exceed 9.78% of the Employee's annual salary."</b>
                </span>
            </div>
        </div>
    </div>
    <!--<h3 style="display:none" id="plan_type_selection"></h3>-->

    <div id="div_family" class="tab container-fluid">
        <hr />
        <div class="row container">
            <div class="col-sm-4">
                <span style="padding-right:12px">Do you have a spouse?</span>
                <input type="radio" id="spouse_yes" name="spouse" value="spouse_yes" onclick="addDependentRows()"/>
                <label for="spouse_yes">Yes</label>
                <input type="radio" id="spouse_no" name="spouse" value="spouse_no" onclick="addDependentRows()"/>
                <label for="spouse_yes">No</label>
            </div>
            <div class="col-sm-8 text-left">
                <label id="num_dependents" style="float:left;margin-right:12px" for="num_children">How many dependent children do you have?</label>
                <select id="num_children" name="num_children" onchange="addDependentRows()" >
                    <option value="0">0</option>
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                    <option value="6">6</option>
                    <option value="7">7</option>
                    <option value="8">8</option>
                    <option value="9">9</option>
                </select>
            </div>
        </div>
    </div>
    <div id="dependent_info_container" class="container-fluid py-2" style="display:none; border:thin solid black">
    </div>
    <div class="row mt-2" id="dependent_info_" style="display:none; padding-bottom:8px; padding-top:4px">
        <hr /> 
        <div class="col-sm-3">
            <input type="radio" class="dependent_spouse" id="dependent_spouse" name="dependents" />
            <label for="dependent_spouse">Spouse</label><br />
            <input type="radio" class="dependent_spouse" id="dependent_child" name="dependents" />
            <label for="dependent_child">Child</label>
        </div>
        <div class="col-sm-3">
            <label style="float:left" for="dependent_name">Dependent Full Name:</label><br />
            <input size="18" type="text" name="dependent_name" />
        </div>
        <div class="col-sm-2">
            <label style="float:left" for="dependent_ssn">Social Security #:</label><br />
            <input size="12" type="text" name="dependent_ssn" />
        </div>
        <div class="col-sm-2">
            <label style="float:left" for="dependent_dob">Date Of Birth:</label><br />
            <input type="date" name="dependent_dob" />
        </div>
        <div class="col-sm-2">
            <label style="padding-left:16px" class="checkbox-inline">
                <input class="dependent_gender" type="radio" name="inp_gender" value="M"/> <span id="gender_male">Male</span>
            </label><br />
            <label style="padding-left:16px" class="checkbox-inline">
                <input class="dependent_gender" type="radio" name="inp_gender" value="F"/><span id="gender_female">Female</span>
            </label>
        </div>
    </div>

    <div id="div_signature" style="display:none" class="container-fluid">
        <hr />
        <h1>
            Sign Below
        </h1>
        <div class="wrapper">
            <canvas style="border:2px solid blue" id="signature-pad" class="signature-pad" width=800 height=300></canvas>
        </div>
        <div>
            <button id="clear">Clear</button>
        </div>
    </div>

    <div class="container-fluid" style="overflow:auto;">
        <div style="float:right;">
            <button type="button" value="prev" disabled="disabled" id="prevBtn" onclick="prev()">Previous</button>
            <button type="button" value="next" disabled="disabled" id="nextBtn" onclick="next()">Next</button>
        </div>
    </div>

        
    <!-- Circles which indicates the steps of the form: -->
    <!--<div style="text-align:center;margin-top:40px;">
        <span class="step"></span>
        <span class="step"></span>
        <span class="step"></span>
        <span class="step"></span>
    </div>-->
</div>
    <!-- Option 1: jQuery and Bootstrap Bundle (includes Popper) -->
      <script  src="https://code.jquery.com/jquery-3.5.1.min.js" integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0=" crossorigin="anonymous"></script>    
      <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ho+j7jyWK8fNQe+A12Hb8AhRq26LrZ/JpcUGGOn+Y7RsweNrtN/tE3MoK7ZeZDyx" crossorigin="anonymous"></script>
      <!-- Digital Signature -->
      <script src="https://cdn.jsdelivr.net/npm/signature_pad@2.3.2/dist/signature_pad.min.js"></script>
      
      <!-- Option 2: jQuery, Popper.js, and Bootstrap JS
    <script src="https://code.jquery.com/jquery-3.5.1.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js" integrity="sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.min.js" integrity="sha384-w1Q4orYjBQndcko6MimVbzY0tgp4pWB4lZ7lr30WKz0vr/aWKhXdBNmNb5D92v7s" crossorigin="anonymous"></script>
    -->
    <script src="../Scripts/ACAForm.js"></script>
  </body>
</html>
