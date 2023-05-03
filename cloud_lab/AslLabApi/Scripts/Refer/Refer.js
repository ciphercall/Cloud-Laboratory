var ReferApp = angular.module("ReferApp", ['ui.bootstrap']);

ReferApp.controller("ApiReferController", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();

        var ReferGroupId = $("#ReferGroupId").val();
        var ReferId = $('#ReferId').val();
        var ReferName = $('#ReferName').val();
       
        var Title = $('#Title').val();
        var Address = $('#Address').val();
        var MobileNo1 = $('#MobileNo1').val();
        var MobileNo2 = $('#MobileNo2').val();
        var EmailId = $('#EmailId').val();
        
        var insltude = $('#latlonPos').val();
        var insuserid = $('#InsUserID').val();

        $http.get('/api/ApiRefer/GetData/', {
            params: {
                Compid: compid,

                ReferGroupId: ReferGroupId,
                ReferId: ReferId,
                ReferName: ReferName,
                Title: Title,
                Address: Address,
                MobileNo1: MobileNo1,
                MobileNo2: MobileNo2,
                EmailId: EmailId,
                Insltude: insltude,
                Insuserid:insuserid


            }
        }).success(function (data) {
            var TCatId = data[0].TCatId;


          

            if (TCatId != 0) {
                $scope.ReferData = data;
            }
            else {
                $scope.ReferData = null;
            }
            //$("#ReferGroupId").val(data[0].ReferGroupId);
            //$('#ReferId').val(data[0].ReferId);
            alert("Successfully Done");
            $("#ReferGroupId").val("");
            $('#ReferId').val("");
            $('#ReferName').val("");
            $('#Title').val("");
            $('#Address').val("");
            $('#MobileNo1').val("");
            $('#MobileNo2').val("");
            $('#EmailId').val("");
            $scope.loading = false;

        });

    };


    $scope.addrow = function () {
        $scope.loading = false;
        event.preventDefault();
      
        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.ReferId = $('#ReferId').val();
        this.newChild.ReferGroupId = $("#ReferGroupId").val();
        
        this.newChild.INSUSERID = $('#InsUserID').val();
        this.newChild.INSLTUDE = $('#latlonPos').val();

        if (this.newChild.ReferId != "") {
            $http.post('/api/grid/ReferChild', this.newChild).success(function (data, status, headers, config) {


                var compid = $('#txtcompid').val();

                var ReferGroupId = $('#ReferGroupId').val();
                var ReferId = $('#ReferId').val();
                var ReferName = $('#ReferName').val();

                var Title = $('#Title').val();
                var Address = $('#Address').val();
                var MobileNo1 = $('#MobileNo1').val();
                var MobileNo2 = $('#MobileNo2').val();
                var EmailId = $('#EmailId').val();
                
                var insltude = $('#latlonPos').val();
                var insuserid = $('#InsUserID').val();
                
                $http.get('/api/ApiRefer/GetData/', {
                    params: {
                        Compid: compid,

                        ReferGroupId: ReferGroupId,
                        ReferId: ReferId,
                        ReferName: ReferName,
                        Title: Title,
                        Address: Address,
                        MobileNo1: MobileNo1,
                        MobileNo2: MobileNo2,
                        EmailId: EmailId,
                        Insltude: insltude,
                        Insuserid: insuserid


                    }
                }).success(function (data) {
                    var TCatId = data[0].TCatId;




                    if (TCatId != 0) {
                        $scope.ReferData = data;
                    }
                    else {
                        $scope.ReferData = null;
                    }

                    $("#ReferGroupId").val(data[0].ReferGroupId);
                    $('#ReferId').val(data[0].ReferId);

                    $scope.loading = false;

                });
              


                if (data.TCatId != 0) {
                    $('#TCatId').val("");
                    $('#ReferPCNT').val("");
                    $('#Remarks').val("");

                    $scope.ReferData.push({ ID: data.ID, TCatId: data.TCatId, ReferPCNT: data.ReferPCNT, Remarks: data.Remarks });
                } else {
                    $('#TCatId').val("");
                    $('#ReferPCNT').val("");
                    $('#Remarks').val("");

                    alert("duplicate name will not create");
                }

               
            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#TCatId').val("");
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
        this.item.ReferGroupId = $('#ReferGroupId').val();
        this.item.ReferId = $('#ReferId').val();

        $http.post('/api/ApiRefer/SaveData', this.item).success(function (data) {
            if (data.TCatId != 0) {
                alert("Saved Successfully!!");
            } else {
                alert("Duplicate data entered");
            }

            frien.editMode = false;
            var compid = $('#txtcompid').val();

            var ReferGroupId = $("#ReferGroupId").val();
            var ReferId = $('#ReferId').val();
            var ReferName = $('#ReferName').val();

            var Title = $('#Title').val();
            var Address = $('#Address').val();
            var MobileNo1 = $('#MobileNo1').val();
            var MobileNo2 = $('#MobileNo2').val();
            var EmailId = $('#EmailId').val();
            var insltude = $('#latlonPos').val();
            var insuserid = $('#InsUserID').val();


            $http.get('/api/ApiRefer/GetData/', {
                params: {
                    Compid: compid,

                    ReferGroupId: ReferGroupId,
                    ReferId: ReferId,
                    ReferName: ReferName,
                    Title: Title,
                    Address: Address,
                    MobileNo1: MobileNo1,
                    MobileNo2: MobileNo2,
                    EmailId: EmailId,
                    Insltude: insltude,
                    Insuserid: insuserid


                }
            }).success(function (data) {
                var TCatId = data[0].TCatId;




                if (TCatId != 0) {
                    $scope.ReferData = data;
                }
                else {
                    $scope.ReferData = null;
                }
                $("#ReferGroupId").val(data[0].ReferGroupId);
                $('#ReferId').val(data[0].ReferId);

            

            });

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

            $.each($scope.ReferData, function (i) {
                if ($scope.ReferData[i].ID === id) {
                    $scope.ReferData.splice(i, 1);
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