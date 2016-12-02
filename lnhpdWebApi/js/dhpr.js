//var dhpr = "http://api.hres.ca/Controllers/dhprController.ashx?";

var dhpr = "./Controllers/dhprController.ashx?";

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
     return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));    
}

function goDhprUrl(lang, pType) {
    var term = getParameterByName("term");
    var searchUrl = dhpr + "term=" + term + "&pType=" + pType + "&lang=" + lang;
    return searchUrl;
}


function goDhprLangUrl(lang, pType) {
    var term = getParameterByName("term");
    var langSwitch = lang == 'en' ? "fr" : "en";
    var langUrl = lang == 'fr' ? "sommaire-decision-reglementaire-resultat.html" : "regulatory-decision-summary-result.html";
    langUrl += "?term=" + term + "&pType=" + pType + "&lang=" + langSwitch;
    return langUrl;
}

function goDhprUrlByID(lang, pType) {
    var linkID = getParameterByName("linkID");
    var searchUrl = dhpr + "linkID=" + linkID +  "&pType=" + pType + "&lang=" + lang;
    return searchUrl;
}
function goDhprLangUrlByID(lang, pType) {
    var linkID = getParameterByName("linkID");
    var langSwitch = lang == 'en' ? "fr" : "en";
    var langUrl;

    if (pType == "lnhpd") {
        return "licensed-natural-health-product-detail-fr.html";
    }
    langUrl = "regulatory-decision-summary-result.html?" + langSwitch + ".html?linkID=" + linkID + "&pType=" + pType + "&lang=" + langSwitch;
    return langUrl;
}


function OnFail(result) {
    window.location.href = "./genericError.html";
}


function formatedContact(contactName, contactUrl) {
    if ($.trim(contactName) == '')
    {
        return "&nbsp;";
    }
    return '<a href='+ contactUrl + '>' + contactName + '</a>';
}


function formatedDate(data) {
        if ($.trim(data) == '') {
            return "";
        }
        var data = data.replace("/Date(", "").replace(")/", "");
        if (data.indexOf("+") > 0) {
            data = data.substring(0, data.indexOf("+"));
        }
        else if (data.indexOf("-") > 0) {
            data = data.substring(0, data.indexOf("-"));
        }
        var date = new Date(parseInt(data, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
}

function formatedList(data) {
    var list;
    if ($.trim(data) == '') {
        return "";
    }
    $.each(data, function (index, record) {
        list += "<li>" + record + "</li>";
    });

    if (list != '') {
        list = list.replace("undefined", "");
        list = list.replace(/"/g, "");
        return "<ul>" + list + "</ul>";;
    }
    return "";
}

function formatedBrandName(brandName, secondaryBrandNameList) {
    if (secondaryBrandNameList == null) {
        return brandName;
    }
    var txt = "";
    var i;
    txt += brandName + ";<br />";
    for (i = 0; i < secondaryBrandNameList.length; i++) {
        if (secondaryBrandNameList[i] != brandName && secondaryBrandNameList[i] != null) {
            txt += secondaryBrandNameList[i] + ";<br />";
        }
    }

    if (txt != '') {
        txt = txt.replace("undefined", "");
        return txt;
    }
    return "&nbsp;";
}

function formatedDoseList(monographFlag, doseList) {
    if (monographFlag >= 1) {
        $("#recommendDoseTable").hide();
        $("#doseAuthMonoLink").show();
        return "";
    } else {
        if (doseList.length == 0) {
            $("#recommendDoseSection").hide();
            return "";
        }
        // console.log(data.length);
        var txt = "";
        var i;
        for (i = 0; i < doseList.length; i++) {
            txt += "<tr><td headers='population subpopulation'>" + (doseList[i].population_type_desc) + "</td>";
            txt += "<td headers='population subpopage'>" + (doseList[i].age) + "</td>";
            txt += "<td headers='population subpopmin'>" + (doseList[i].age_minimum) + "</td>";
            txt += "<td headers='population subpopmax'>" + (doseList[i].age_maximum) + "</td>";
            txt += "<td headers='population subpopuomage'>" + (doseList[i].age_uom_type_desc) + "</td>";

            txt += "<td headers='quantity qty'>" + (doseList[i].quantity_dose) + "</td>";
            txt += "<td headers='quantity qtymin'>" + (doseList[i].quantity_dose_minimum) + "</td>";
            txt += "<td headers='quantity qtymax'>" + (doseList[i].quantity_dose_maximum) + "</td>";
            txt += "<td headers='quantity uomqty'>" + (doseList[i].quantity_dose_uom_type_desc) + "</td>";

            txt += "<td headers='frequency freq'>" + (doseList[i].frequency) + "</td>";
            txt += "<td headers='frequency freqmin'>" + (doseList[i].frequency_minimum) + "</td>";
            txt += "<td headers='frequency freqmax'>" + (doseList[i].frequency_maximum) + "</td>";
            txt += "<td headers='frequency uomfreq'>" + (doseList[i].frequency_uom_type_desc);
            txt += "</td></tr>"
        }

        if (txt != '') {
            txt = txt.replace("undefined", "");
            $("#doseAuthMonoLink").hide();
            $("#recommendDoseTable").show();
            return txt;
        }
        $("#recommendDoseSection").hide();
        return "&nbsp;";
    }
}

function formatedMedIngList(monographFlag, miList) {
    if (miList.length == 0) {
        $("#medIngTable").hide();
        return "";
    }

    if (miList.length == 1) {
        $("#medIngTable").hide();
        return miList[0].ingredient_name;
    }

    var txt = '';
    var i;
    for (i = 0; i < miList.length; i++) {
        txt += '<tr><td headers="ingredient">' + miList[i].ingredient_name + "</td>";
        if (monographFlag >= 1) {
            return '<tr><td colspan="3"><a href="http://webprod.hc-sc.gc.ca/nhpid-bdipsn/monosReq.do?lang=eng">As authorized in the NHPD monograph(s) to which the applicant attested</a></td>';
        } else {
            for (j = 0; j < miList[i].quantity_list.length; j++) {
                txt += '<td headers="ingquantity">';
                if (miList[i].quantity_list[j].Quantity != 0) {
                    txt += miList[i].quantity_list[j].quantity_string;
                }

                txt += "</td>";
                
                txt += '<td headers="extract">';
                if (miList[i].quantity_list[j].ratio_numerator != 0) {
                    txt += miList[i].quantity_list[j].extract_string;
                }
                txt += "</td>";
                    
                txt += '<td headers="potency">';
                if (miList[i].quantity_list[j].potency_string != "") {
                    txt += miList[i].quantity_list[j].potency_string;
                }
                txt += "</td></tr>";
            }
        }
    }

    if (txt != '') {
        txt = txt.replace("undefined", "");
        return txt;
    }

    return "&nbsp;";
}

function formatedNonMedIngList(nmiList) {
    if (nmiList.length == 0) {
        $("#nonMedIngSection").hide();
        return "";
    }
    
    if (nmiList.length == 1) {
        $("#nonMedIngSection").hide();
        return nmiList[0].ingredient_name;
    }
    
    var txt = "<ul>";
    var i;
    for (i = 0; i < nmiList.length; i++) {
        txt += "<li>" + nmiList[i].ingredient_name + "</li>";
    }
    
    if (txt != '') {
        txt = txt.replace("undefined", "");
        return txt + "</ul>";
    }
    return "&nbsp;";
}

function formatedProductStatus(lang, flagProductStatus) {
    if ($.trim(flagProductStatus) == '') {
        return "";
    }

    switch (flagProductStatus) {
        case 0:
            return lang == "fr" ? "Discontinué" : "Discontinued";
        case 1:
            return lang == "fr" ? "Actif" : "Active";
        case 2:
            return lang == "fr" ? "Cessation de vente" : "Stop Sale";
        case 3:
            return lang == "fr" ? "Suspendu" : "Suspended";
        case 4:
            return lang == "fr" ? "Annulé" : "Cancelled";
        case 5:
            return lang == "fr" ? "Toutes" : "All";
        default:
            return "&nbsp;";
    }
  
}

function formatedPurposeList(monographFlag, purposeList) {
    if (monographFlag >= 1) {
        $("#recommendPurpose").hide();
        $("#purposeAuthMonoLink").show();
        return "";
    } else {
        if (purposeList.length == 0) {
            $("#recommendPurposeSection").hide();
            return "";
        }

        var txt = "";
        var i;
        for (i = 0; i < purposeList.length; i++) {
            txt += purposeList[i].purpose + "<br />";
        }

        if (txt != '') {
            txt = txt.replace("undefined", "");
            $("#recommendPurpose").show();
            $("#purposeAuthMonoLink").hide();
            return txt;
        }
        return "&nbsp;";
    }
}

function formatedRiskList(monographFlag, riskList) {
    if (monographFlag >= 1) {
        $("#riskInfo").hide();
        $("#riskAuthMonoLink").show();
        return "";
    } else {
        if (riskList.length == 0) {
            $("#riskInfoSection").hide();
            return "";
        }

        var txt = "";
        var i;
        for (i = 0; i < riskList.length; i++) {
            txt += riskList[i].risk_type_desc + "&nbsp;" + riskList[i].sub_risk_type_desc + "<br />";
            for (j = 0; j < riskList[i].risk_text_list.length; j++) {
                txt += "<dd>" + riskList[i].risk_text_list[j].risk_text + "</dd>";
            }
        }

        if (txt != '') {
            txt = txt.replace("undefined", "");
            $("#riskInfo").show();
            $("#riskAuthMonoLink").hide();
            return txt;
        }
        return "&nbsp;";
    }
}


function formatedRouteList(routeList) {
    if (routeList.length == null) {
        $("#routeAdmin").hide();
        return "";
    }

    var txt = "";
    var i;
    for (i = 0; i < routeList.length; i++) {
        txt += routeList[i].route_type_desc;
    }

    if (txt != '') {
        txt = txt.replace("undefined", "");
        return txt;
    }

    return "&nbsp;";
}

function formatedRoute(lang, npn) {

    if ($.trim(npn) == '') {
        return "";
    }

    return UtilityHelper.GetProductRoutesByID(npn, lang);
}


function getUnique(inputArray) {
    var outputArray = [];

    for (var i = 0; i < inputArray.length; i++) {
        if ((jQuery.inArray(inputArray[i], outputArray)) == -1) {
            outputArray.push(inputArray[i]);
        }
    }
    return outputArray;
}

function objectFindByKey(array, key, value) {
    for (var i = 0; i < array.length; i++) {
        if (array[i][key] === value) {
            return array[i];
        }
    }
    return null;
}

function formatedArrayList(data) {
    
    var list;
    if ($.trim(data) == '') {
        return "";
    }


    var arraryCount = 0;
    var returnValue = "";
    var orderNoList = [];

    for (i = 0; i < data.length; i++) {
        orderNoList[i] = $.trim(data[i].OrderNo);
    }
    var uniqueList = getUnique(orderNoList);  
   // console.log(uniqueList.length);

    for (var i = 0; i < uniqueList.length; i++) {
        var title = "";
        var list = "";
        //console.log("arraryCount" + arraryCount);
        //console.log("uniqueList.length" + uniqueList.length);
        //console.log("uniqueList" + uniqueList[i]);
        //console.log("i" + i);
        if (arraryCount == i)
        {
            title = "<br/><strong> Conclusion" + uniqueList[i] + "</strong><br/>";
            //var found = $.map(data, function (val) {
            //    return val.OrderNo == uniqueList[i] ? val.Bullet + "<br/>" : null;
            //});
            var result = $.grep(data, function (e) {
                if (e.OrderNo == uniqueList[i])
                {
                    list += e.Bullet + "<br/>";
                } 
            });
        }
        returnValue += title + list;
        arraryCount ++;
    }
    //console.log("returnValue" + returnValue);
    return returnValue;   
}


function ExportExcel(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData.data) : JSONData.data;
    var CSV = '';
    //Set Report title in first row or line

    CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";
        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {
            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);
        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            if (index == 'DateDecision' || index == 'CreatedDate') {
                row += '"' + formatedDate(arrData[i][index]) + '",';
            }
            else {
                row += '"' + arrData[i][index] + '",';
            }
        }

        row.slice(0, row.length - 1);
        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "RDSResult_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function ExportJson(el, JSONData) {
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var data = "text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(arrData));
    el.setAttribute("href", "data:" + data);
    el.setAttribute("download", "RDSResult.json");
}

//Export Xml
function ExportXml(el) {
    var JSONData = $('#txt').val();
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var xml = json2xml(arrData, 'items');
    xml = '<?xml version="1.0" encoding="utf-8"?>' + xml;    
    var data = "Application/octet-stream," + encodeURIComponent(xml);
    el.setAttribute("href", "data:" + data);
    el.setAttribute("download", "RDSResult.xml");
}

var arrayCount = 0;
var json2xml = (function (undefined) {
    "use strict";
    var tag = function (name, closing) {
       // console.log("name:" + name + "arrayCount:" + arrayCount);
        if (name == arrayCount) {
            name = "item";
        }
        return "<" + (closing ? "/" : "") + name + ">";
    };
    return function (obj, rootname) {
        var xml = "";
        for (var i in obj) {

            if (obj.hasOwnProperty(i)) {
                var value = obj[i],
                    type = typeof value;
                if (value instanceof Array && type == 'object') {
                    for (var sub in value) {
                        xml += json2xml(value[sub]);
                    }
                } else if (value instanceof Object && type == 'object') {
                    xml += tag(i) + json2xml(value) + tag(i, 1);
                    arrayCount++;                   
                } else {
                    if (i == 'DateDecision' || i == 'CreatedDate') {
                        xml += tag(i) + formatedDate(value) + tag(i, {
                            closing: 1
                        });
                    }
                    else
                    {
                        xml += tag(i) + value + tag(i, {
                            closing: 1
                        });
                    }
                }
                
            }
        }

        return rootname ? tag(rootname) + xml + tag(rootname, 1) : xml;
    };
})(json2xml || {});



   

