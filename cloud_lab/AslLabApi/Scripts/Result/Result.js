var resultApp = angular.module("resultApp", ['ui.bootstrap']);


resultApp.controller("ApiResultController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = true;





    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();
        var transdt = $('#TransactionDate').val();
        var patientid = $("#PatientId").val();
        var testid = $('#TestId').val();
        var insuserid = $('#InsUserID').val();
        var insltude = $('#latlonPos').val();

        $http.get('/api/ApiResult/GetData/', {
            params: {
                Compid: compid,
                Transdt: transdt,
                Patientid: patientid,
                Testid: testid,
                Insuserid:insuserid,
                Insltude: insltude


            }
        }).success(function (data) {
            var restid = data[0].RestId;
         
      

            if (restid != 0) {
                $scope.ResultData = data;
            }
            else {
                $scope.ResultData = null;
            }

            $scope.loading = false;

        });

    };


    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
        

        

    };

    $scope.cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        var compid = $('#txtcompid').val();
        var transdt = $('#TransactionDate').val();
        var patientid = $("#PatientId").val();
        var testid = $('#TestId').val();

        $http.get('/api/ApiResult/GetData/', {
            params: {
                Compid: compid,
                Transdt: transdt,
                Patientid: patientid,
                Testid: testid


            }
        }).success(function (data) {
            var restid = data[0].RestId;



            if (restid != 0) {
                $scope.ResultData = data;
            }
            else {
                $scope.ResultData = null;
            }

            $scope.loading = false;

        });


    };


    //Used to save a record after edit
    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.testitem;
        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.TransactionDate = $('#TransactionDate').val();
        this.testitem.PatientId = $("#PatientId").val();
        this.testitem.TestId = $('#TestId').val();
        this.testitem.INSUSERID = $('#InsUserID').val();
        this.testitem.INSLTUDE = $('#latlonPos').val();
      
        $http.post('/api/ApiResult/SaveData', this.testitem).success(function (data) {
            if (data.Result != "") {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }

            frien.editMode = false;

            var compid = $('#txtcompid').val();
            var transdt = $('#TransactionDate').val();
            var patientid = $("#PatientId").val();
            var testid = $('#TestId').val();

            $http.get('/api/ApiResult/GetData/', {
                params: {
                    Compid: compid,
                    Transdt: transdt,
                    Patientid: patientid,
                    Testid: testid


                }
            }).success(function (data) {
                var restid = data[0].RestId;



                if (restid != 0) {
                    $scope.ResultData = data;
                }
                else {
                    $scope.ResultData = null;
                }

              

            });
       

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };





    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();
        //var abc = this.newChild;
        this.newChild.TransactionDate = $('#TransactionDate').val();
        this.newChild.PatientId = $('#PatientId').val();
        this.newChild.TestId = $('#TestId').val();
        
        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();
        this.newChild.INSUSERID = $('#InsUserID').val();
        if (this.newChild.TransactionDate != "" && this.newChild.PatientId!="") {
            $http.post('/api/ApiResult/Child', this.newChild).success(function (data, status, headers, config) {


                var compid = $('#txtcompid').val();
                var transdt = $('#TransactionDate').val();
                var patientid = $("#PatientId").val();
                var testid = $('#TestId').val();

                $http.get('/api/ApiResult/GetData/', {
                    params: {
                        Compid: compid,
                        Transdt: transdt,
                        Patientid: patientid,
                        Testid: testid


                    }
                }).success(function (data) {
                    var restid = data[0].RestId;



                    if (restid != 0) {
                        $scope.ResultData = data;
                    }
                    else {
                        $scope.ResultData = null;
                    }

                    $scope.loading = false;

                });


                if (data.TestId != 0) {
                  
                    $('#Result').val("");
                  
                    $scope.ResultData.push({ ID: data.ID, RestId: data.RestId ,Result:data.Result});
                } else {
                  
                    $('#Result').val("");
                   
                    alert("duplicate name will not create");
                }

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            
            alert("Enter Master Data First");
        }

    };


    $scope.deleteTestitem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;
        $http.post('/api/ApiResult/DeleteData/', this.testitem).success(function (data) {

            $.each($scope.ResultData, function (i) {
                if ($scope.ResultData[i].ID === id) {
                    $scope.ResultData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };





});
