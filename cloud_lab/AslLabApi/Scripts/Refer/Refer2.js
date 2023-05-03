var ReferApp2 = angular.module("ReferApp2", ['ui.bootstrap']);

ReferApp2.controller("ApiReferController2", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;


        //$scope.loading = true;

       


        var compid = $('#txtcompid').val();

        //var ReferGroupId = $("#ReferGroupId").val();
        //var ReferId = $('#ReferId').val();
        //var ReferName = $('#ReferName').val();

        //var Title = $('#Title').val();
        //var Address = $('#Address').val();
        //var MobileNo1 = $('#MobileNo1').val();
        //var MobileNo2 = $('#MobileNo2').val();
        //var EmailId = $('#EmailId').val();

        $http.get('/api/ApiRefer/GetData2/', {
            params: {
                Compid: compid,

                //ReferGroupId: ReferGroupId,
                //ReferId: ReferId,
                //ReferName: ReferName,
                //Title: Title,
                //Address: Address,
                //MobileNo1: MobileNo1,
                //MobileNo2: MobileNo2,
                //EmailId: EmailId


            }
        }).success(function (data) {
            var ReferId = data[0].ReferId;




            if (ReferId != 0) {
                $scope.ReferData2 = data;
            }
            else {
                $scope.ReferData2 = null;
            }
            //$("#ReferGroupId").val(data[0].ReferGroupId);
            //$('#ReferId').val(data[0].ReferId);

            $scope.loading = false;

        });

 


  

    $scope.toggleEdit = function () {
        this.item.editMode = !this.item.editMode;

    };

    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.item;
        this.item.COMPID = $('#txtcompid').val();
      
        this.item.UPDUSERID = $('#Upuserid').val();
        this.item.UPDLTUDE = $('#latlonPos').val();

        $http.post('/api/ApiRefer/SaveData2', this.item).success(function (data) {
            if (data.TCatId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }

            frien.editMode = false;


            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };

    $scope.deleteitem = function () {
        $scope.loading = true;
        var id = this.item.ID;
        $http.post('/api/ApiRefer/DeleteData/', this.item).success(function (data) {

            $.each($scope.ReferData2, function (i) {
                if ($scope.ReferData2[i].ID === id) {
                    $scope.ReferData2.splice(i, 1);
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