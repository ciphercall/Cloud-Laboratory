var MgtApp = angular.module("MgtApp", ['ui.bootstrap']);

MgtApp.controller("ApiMgtController", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();

    
        var ManagementId = $('#ManagementId').val();
        var ManagementName = $('#ManagementName').val();

        var Designation = $('#Designation').val();
        var Address = $('#Address').val();
        var MobileNo1 = $('#MobileNo1').val();
        var MobileNo2 = $('#MobileNo2').val();
        var EmailId = $('#EmailId').val();
        var insltude = $('#latlonPos').val();
        var insuserid = $('#InsUserID').val();


        $http.get('/api/ApiMgt/GetData/', {
            params: {
                Compid: compid,
                ManagementId: ManagementId,
                ManagementName: ManagementName,
                Designation: Designation,
                Address: Address,
                MobileNo1: MobileNo1,
                MobileNo2: MobileNo2,
                EmailId: EmailId,
                Insltude: insltude,
                Insuserid: insuserid


            }
        }).success(function (data) {
            var ReferId = data[0].ReferId;




            if (ReferId != 0) {
                $scope.MgtData = data;
            }
            else {
                $scope.MgtData = null;
            }
           
            $('#ManagementId').val(data[0].ManagementId);

            $scope.loading = false;

        });

    };


    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();

        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.ManagementId = $('#ManagementId').val();
        this.newChild.INSUSERID = $('#InsUserID').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();

        if (this.newChild.ManagementId != "") {
            $http.post('/api/grid/MgtChild', this.newChild).success(function (data, status, headers, config) {


                var compid = $('#txtcompid').val();


                var ManagementId = $('#ManagementId').val();
                var ManagementName = $('#ManagementName').val();

                var Designation = $('#Designation').val();
                var Address = $('#Address').val();
                var MobileNo1 = $('#MobileNo1').val();
                var MobileNo2 = $('#MobileNo2').val();
                var EmailId = $('#EmailId').val();
                var insltude = $('#latlonPos').val();
                var insuserid = $('#InsUserID').val();

                $http.get('/api/ApiMgt/GetData/', {
                    params: {
                        Compid: compid,
                        ManagementId: ManagementId,
                        ManagementName: ManagementName,
                        Designation: Designation,
                        Address: Address,
                        MobileNo1: MobileNo1,
                        MobileNo2: MobileNo2,
                        EmailId: EmailId,
                        Insltude: insltude,
                        Insuserid: insuserid


                    }
                }).success(function (data) {
                    var ReferId = data[0].ReferId;




                    if (ReferId != 0) {
                        $scope.MgtData = data;
                    }
                    else {
                        $scope.MgtData = null;
                    }

                    $('#ManagementId').val(data[0].ManagementId);

                    $scope.loading = false;

                });

                if (data.ReferId != 0) {
                    $('#ReferId').val("");
                    $('#ReferPCNT').val("");
                    $('#Remarks').val("");

                    $scope.MgtData.push({ ID: data.ID, ReferId: data.ReferId, ReferPCNT: data.ReferPCNT, Remarks: data.Remarks });
                } else {
                    $('#ReferId').val("");
                    $('#ReferPCNT').val("");
                    $('#Remarks').val("");

                    alert("duplicate name will not create");
                }


            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#ReferId').val("");
            $('#ReferPCNT').val("");
            $('#Remarks').val("");

            alert("Enter Master Data First");
        }

    };

    $scope.toggleEdit = function () {
        this.item.editMode = !this.item.editMode;

    };

    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.item;
        this.item.COMPID = $('#txtcompid').val();
      
        this.item.ManagementId = $('#ManagementId').val();

        $http.post('/api/ApiMgt/SaveData', this.item).success(function (data) {
            if (data.ReferId != 0) {
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
        $http.post('/api/ApiMgt/DeleteData/', this.item).success(function (data) {

            $.each($scope.MgtData, function (i) {
                if ($scope.MgtData[i].ID === id) {
                    $scope.MgtData.splice(i, 1);
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