var testmasterApp2 = angular.module("testmasterApp2", ['ui.bootstrap']);

testmasterApp2.controller("ApiTestMasterController2", function ($scope, $http) {

    var compid = $('#txtcompid').val();
    $http.get('/api/ApiTestMaster/HeadData/', {
        params: {
            Compid: compid,
           


        }
    }).success(function (data) {
      
      

     
        $scope.TestMasterHead = data;
       

        $scope.loading = false;

    });

    $scope.toggleEdit = function () {
        this.testmaster.editMode = !this.testmaster.editMode;

    };

    $scope.toggleEdit_Cancel = function () {
        this.testmaster.editMode = !this.testmaster.editMode;

     
        var compid = $('#txtcompid').val();
        $http.get('/api/ApiTestMaster/HeadData/', {
            params: {
                Compid: compid,



            }
        }).success(function (data) {




            $scope.TestMasterHead = data;


            $scope.loading = false;

        });

    };

    $scope.save = function () {
       
        $scope.loading = true;
        var frien = this.testmaster;
        this.testmaster.COMPID = $('#txtcompid').val();
        //this.testmaster.TestCatName = $('#gridTestCatName').val();
        this.testmaster.UPDLTUDE = $('#latlonPos').val();
        this.testmaster.UPDUSERID = $('#UpUserID').val();
        
        $http.post('/api/ApiTestMasterHead/SaveData2', this.testmaster).success(function (data) {
            if (data.TestId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }

            frien.editMode = false;

            var compid = $('#txtcompid').val();
            $http.get('/api/ApiTestMaster/HeadData/', {
                params: {
                    Compid: compid,



                }
            }).success(function (data) {




                $scope.TestMasterHead = data;


              

            });

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };

});