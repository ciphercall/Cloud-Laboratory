var testVApp = angular.module("testVApp", ['ui.bootstrap']);

testVApp.controller("ApiTestVController", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();
      
        var catid = $("#testCatId").val();
        var testid = $('#TestId').val();

        $http.get('/api/ApiTestV/GetData/', {
            params: {
                Compid: compid,
              
                TCatID: catid,
                TestID: testid


            }
        }).success(function (data) {
            var TestVacID = data[0].TestVACID;
          

          //  $('#TestCatId').val(TCatID);

            if (TestVacID != 0) {
                $scope.TestVData = data;
            }
            else {
                $scope.TestVData = null;
            }

            $scope.loading = false;

        });

    };


    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();
        this.newChild.testCatId = $('#testCatId').val();
        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.TestId = $('#TestId').val();
        
        this.newChild.INSUSERID = $('#InsUserID').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();
      
        if (this.newChild.testCatId != "") {
            $http.post('/api/grid/TestVChild', this.newChild).success(function (data, status, headers, config) {


                var compid = $('#txtcompid').val();

                var catid = $("#testCatId").val();
                var testid = $('#TestId').val();

                $http.get('/api/ApiTestV/GetData/', {
                    params: {
                        Compid: compid,

                        TCatID: catid,
                        TestID: testid


                    }
                }).success(function (data) {
                    var TestVacID = data[0].TestVACID;


                    //  $('#TestCatId').val(TCatID);

                    if (TestVacID != 0) {
                        $scope.TestVData = data;
                    }
                    else {
                        $scope.TestVData = null;
                    }

                    $scope.loading = false;

                });
              


                if (data.TestVACID != 0) {
                    $('#TestVACID').val("");
                   
                    $scope.TestVData.push({ ID: data.ID, TestCatId: data.TestCatId, TestId: data.TestId, TestVACID: data.TestVACID });
                } else {
                    $('#TestVACID').val("");
                   
                    alert("duplicate name will not create");
                }

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#TestVACID').val("");
          
            alert("Enter Test V Master Data First");
        }

    };

    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;

    };

    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.testitem;
        this.testitem.COMPID = $('#txtcompid').val();
       

        $http.post('/api/ApiTestV/SaveData', this.testitem).success(function (data) {
            if (data.TestVACID != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }

            frien.editMode = false;

            var compid = $('#txtcompid').val();

            var catid = $("#testCatId").val();
            var testid = $('#TestId').val();

            $http.get('/api/ApiTestV/GetData/', {
                params: {
                    Compid: compid,

                    TCatID: catid,
                    TestID: testid


                }
            }).success(function (data) {
                var TestVacID = data[0].TestVACID;


                //  $('#TestCatId').val(TCatID);

                if (TestVacID != 0) {
                    $scope.TestVData = data;
                }
                else {
                    $scope.TestVData = null;
                }

                //$scope.loading = false;

            });

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };

    $scope.deleteTestitem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;
        $http.post('/api/ApiTestV/DeleteData/', this.testitem).success(function (data) {

            $.each($scope.TestVData, function (i) {
                if ($scope.TestVData[i].ID === id) {
                    $scope.TestVData.splice(i, 1);
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