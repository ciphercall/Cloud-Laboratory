var testmasterApp = angular.module("testmasterApp", ['ui.bootstrap']);


testmasterApp.controller("ApiTestMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = true;

    

   

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();
        var catname = $('#TestCatName').val();
        var catid = $("#TestCatId").val();
        var dept = $('#DepartmentId').val();
        var topyn = $('#TopYN').val();
        var insltude = $('#latlonPos').val();
        var inuserid = $('#InsUserID').val();

        $http.get('/api/ApiTestMaster/GetData/', {
            params: {
                Compid:compid,
                TCatName: catname,
                TCatID: catid,
                Department: dept,
                Topyn:topyn,
                Insltude: insltude,
                Inuserid:inuserid

            }
        }).success(function (data) {
            var TCatID = data[0].TestCatId;
            var TestID = data[0].TestId;

            $('#TestCatId').val(TCatID);

            if (TCatID != 0 && TestID != 0) {
                $scope.TestMasterData = data;
            }
            else
            {
                $scope.TestMasterData = null;
            }

            $scope.loading = false;

        });

    };
  

    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;

    };

  

    //Used to save a record after edit
    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.testitem;
        this.testitem.COMPID = $('#txtcompid').val();
       
        $http.post('/api/ApiTestMaster/SaveData', this.testitem).success(function (data) {
            if (data.TestId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }
           
            frien.editMode = false;
           

            var compid = $('#txtcompid').val();
            var catname = $('#TestCatName').val();
            var catid = $("#TestCatId").val();
            var dept = $('#DepartmentId').val();

            $http.get('/api/ApiTestMaster/GetData/', {
                params: {
                    Compid: compid,
                    TCatName: catname,
                    TCatID: catid,
                    Department: dept


                }
            }).success(function (data) {
                var TCatID = data[0].TestCatId;
                var TestID = data[0].TestId;

                $('#TestCatId').val(TCatID);

                if (TCatID != 0 && TestID != 0) {
                    $scope.TestMasterData = data;
                }
                else {
                    $scope.TestMasterData = null;
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
        this.newChild.TestCatId = $('#TestCatId').val();
        this.newChild.DepartmentId = $('#DepartmentId').val();
        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();
        this.newChild.INSUSERID = $('#InsUserID').val();


        if (this.newChild.TestCatId != "") {
            $http.post('/api/ApiTestMaster/TestChild', this.newChild).success(function (data, status, headers, config) {


                var compid = $('#txtcompid').val();
                var catname = $('#TestCatName').val();
                var catid = $("#TestCatId").val();
                var dept = $('#DepartmentId').val();
                var topyn = $('#TopYN').val();
                var insltude = $('#latlonPos').val();
                var inuserid = $('#InsUserID').val();
                
                $http.get('/api/ApiTestMaster/GetData/', {
                    params: {
                        Compid: compid,
                        TCatName: catname,
                        TCatID: catid,
                        Department: dept,
                        Topyn: topyn,
                        Insltude: insltude,
                        Inuserid: inuserid


                    }
                }).success(function (data) {
                    var TCatID = data[0].TestCatId;
                    var TestID = data[0].TestId;

                    $('#TestCatId').val(TCatID);

                    if (TCatID != 0 && TestID != 0) {
                        $scope.TestMasterData = data;
                    }
                    else {
                        $scope.TestMasterData = null;
                    }

                    //$scope.loading = false;

                });


                if (data.TestId != 0) {
                    $('#testnm').val("");
                    $('#rate').val("");
                    $('#pcntd').val("");
                    $scope.TestMasterData.push({ ID: data.ID, TestCatId: data.TestCatId, DepartmentId: data.DepartmentId, TestId: data.TestId, TestName: data.TestName, Rate: data.Rate, PcnTD: data.PcnTD });
                } else {
                    $('#testnm').val("");
                    $('#rate').val("");
                    $('#pcntd').val("");
                    alert("duplicate name will not create");
                }

            }).error(function() {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#testnm').val("");
            $('#rate').val("");
            $('#pcntd').val("");
            alert("Enter Test Master Data First");
        }

    };


    $scope.deleteTestitem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;
        $http.post('/api/ApiTestMaster/DeleteData/', this.testitem).success(function (data) {
          
            $.each($scope.TestMasterData, function (i) {
                if ($scope.TestMasterData[i].ID === id) {
                    $scope.TestMasterData.splice(i, 1);
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
