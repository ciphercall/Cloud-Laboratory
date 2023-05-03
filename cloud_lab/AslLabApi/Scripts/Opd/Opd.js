var OpdApp = angular.module("OpdApp", ['ui.bootstrap']);

OpdApp.controller("ApiOpdController", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;
   

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();

        var TransactionDate = $("#TransactionDate").val();
        var patientName = $('#PatientName').val();
        var RFPercentage = $('#RfPercentage').val();
        var patientid = $('#PatientID').val();
        var inUserID = $('#InUserID').val();
        
        var insltude = $('#latlonPos').val();

        $http.get('/api/ApiOpd/GetData/', {
            params: {
                Compid: compid,

                TransactionDate: TransactionDate,
                PatientName: patientName,
                RfPercentage: RFPercentage,
                PatienId: patientid,
                InUserID: inUserID,
                InsLtude: insltude

            }
        }).success(function (data) {
            var TestSerial = data[0].TestSerial;




            if (TestSerial != 0) {
                $scope.Opdata = data;
            }
            else {
                $scope.Opdata = null;
            }
          
            $('#PatientID').val(data[0].PatientID);

            $scope.loading = false;

        });

    };


    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();

        this.newChild.CompanyID = $('#txtcompid').val();
        this.newChild.TransactionDate = $('#TransactionDate').val();
        this.newChild.PatientID = $("#PatientID").val();
        this.newChild.PatientName = $("#PatientName").val();
        this.newChild.Refername = $("#ReferName").val();
        this.newChild.TestCategoryName = $("#TCatId").val();
        
        this.newChild.TestCategoryId = $("#HiddentCatId").val();
        this.newChild.TestID = $("#TestID").val();
        this.newChild.TestName = $("#TestName").val();
        this.newChild.Amount = $("#Amount").val();

        this.newChild.Pcntr = $("#Pcntr").val();

        this.newChild.Pcntd = $("#Pcntd").val();

        this.newChild.Discr = $("#Discr").val();
        
        this.newChild.INSUSERID = $('#InUserID').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();

        if (this.newChild.TransactionDate != "") {
            $http.post('/api/grid/OpdChild', this.newChild).success(function (data, status, headers, config) {

                var compid = $('#txtcompid').val();

                var TransactionDate = $("#TransactionDate").val();
                var patientName = $('#PatientName').val();
                var RFPercentage = $('#RfPercentage').val();
                var patientid = $('#PatientID').val();

                var inUserID = $('#InUserID').val();
        
                var insltude = $('#latlonPos').val();
                $http.get('/api/ApiOpd/GetData/', {
                    params: {
                        Compid: compid,

                        TransactionDate: TransactionDate,
                        PatientName: patientName,
                        RfPercentage: RFPercentage,
                        PatienId: patientid,
                        InUserID: inUserID,
                        InsLtude: insltude

                    }
                }).success(function (data) {
                    var TestSerial = data[0].TestSerial;




                    if (TestSerial != 0) {
                        $scope.Opdata = data;
                    }
                    else {
                        $scope.Opdata = null;
                    }

                    //$('#PatientID').val(data[0].PatientID);

                    $scope.loading = false;

                });

           



                if (data.TestCategoryId != 0) {

                    $("#TestName").val("");
                    $("#TCatId").val("");
                    $('#TestID').val("");
                    $('#HiddentCatId').val("");
                    $('#Amount').val("");
                  
                    $('#Pcntd').val("");
                    $('#Discr').val("");
                    

                //$scope.Opdata.push({ ID: data.ID, TCatId: data.TCatId, ReferPCNT: data.ReferPCNT, Remarks: data.Remarks });
                } else {
                    $("#TestName").val("");
                    $("#TCatId").val("");
                    $('#TestID').val("");
                    $('#HiddentCatId').val("");
                    $('#Amount').val("");
                   
                    $('#Pcntd').val("");
                    $('#Discr').val("");
                    alert("duplicate name will not create");
                }


            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            //$('#TCatId').val("");
            //$('#ReferPCNT').val("");
            //$('#Remarks').val("");

            alert("Enter Master Data First");
        }

    };

   


    $scope.deleteitem = function () {
        $scope.loading = true;
        var id = this.item.ID;
        $http.post('/api/ApiOpd/DeleteData/', this.item).success(function (data) {
           
            $.each($scope.Opdata, function (i) {
                if ($scope.Opdata[i].ID === id) {
                    $scope.Opdata.splice(i, 1);
                    return false;
                }
            });

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };

    $scope.add2ndPart = function (event) {
        $scope.loading = true;

        //event.preventDefault();


        var compid = $('#txtcompid').val();
        var TransactionDate = $("#TransactionDate").val();
        var patientid = $('#PatientID').val();
        var Gender = $('#Gender').val();
        var Age = $("#Age").val();
        var Address = $('#Address').val();
        var MObileNo = $('#MObileNo').val();
        var DOCTORID = $("#DoctorID").val();
        var ReferID = $("#ReferID").val();
        var DvDate = $('#DvDate').val();
        var Dvtm = $('#Dvtm').val();
        var TotalAmount = $("#TotalAmount").val();
        var DiscountRefer = $('#DiscountRefer').val();
        var DiscountHos = $('#DiscountHos').val();
        var Discountnet = $("#Discountnet").val();
        var NetAmount = $('#NetAmount').val();  
        var ReceiveAmount = $("#ReceiveAmount").val();
        var DueAmount = $('#DueAmount').val();
        
       
            $http.get('/api/ApiOpd/Save2ndPart/', {
                params: {
                    Compid: compid,
                    TransactionDate: TransactionDate,
                    PatienId: patientid,
                    Gender: Gender,
                    Age: Age,
                    Address: Address,
                    MObileNo: MObileNo,
                    DoctorID:DOCTORID,
                    ReferID: ReferID,
                    DvDate: DvDate,
                    Dvtm: Dvtm,
                    TotalAmount: TotalAmount,
                    DiscountRefer: DiscountRefer,
                    DiscountHos: DiscountHos,
                    Discountnet: Discountnet,
                    NetAmount: NetAmount,
                    ReceiveAmount: ReceiveAmount,
                    DueAmount: DueAmount


                }
            }).success(function (data) {


                //alert("Saved");
                $("#refresh").click();



                $scope.loading = false;

            });
      
 


    };
    
    $scope.GetSummOfAmount = function (Opdata) {
        var summ = 0;
        for (var i in Opdata) {
            summ = summ + Number(Opdata[i].Amount);
        }
        return summ;
    };
    
    $scope.GetSummOfDiscr = function (Opdata) {
        var summ = 0;
        for (var i in Opdata) {
            summ = summ + Number(Opdata[i].Discr);
        }
        return summ;
    };
    

});