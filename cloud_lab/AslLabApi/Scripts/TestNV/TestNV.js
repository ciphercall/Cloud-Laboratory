var testNVApp = angular.module("testNVApp", ['ui.bootstrap']);

testNVApp.controller("ApiTestNVController", function ($scope, $http) {

    //$scope.loading = false;
    //$scope.addMode = false;
    var flag = 0;

    $scope.add = function (event) {
        $scope.loading = true;

        event.preventDefault();


        var compid = $('#txtcompid').val();

        //var Restid = $('#RestId').val();
        var TestId = $('#TestId').val();

       
            $http.get('/api/ApiTestNV/GetData/', {
                params: {
                    Compid: compid,

                    testid: TestId,
                    //restid: Restid


                }
            }).success(function (data) {
                var RestGname = data[0].RestGroupName;


                //  $('#TestCatId').val(TCatID);

                if (RestGname != "") {
                    $scope.TestNVData = data;
                }
                else {
                    $scope.TestNVData = null;
                }

                $scope.loading = false;

            });
       
       

    };


    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();
        this.newChild.RestId = $('#RestId').val();
        this.newChild.COMPID = $('#txtcompid').val();
        this.newChild.TestId = $('#TestId').val();
        this.newChild.RestName = $('#RestName').val();
        
        this.newChild.INSLTUDE = $('#latlonPos').val();
        this.newChild.INSUSERID = $('#InsUserID').val();

        if (this.newChild.TestId!="") {
            $http.post('/api/grid/TestNVChild', this.newChild).success(function (data, status, headers, config) {

                var compid = $('#txtcompid').val();

                $("#RestId").val(data.RestId);
                //var Restid = $("#RestId").val();
                var TestId = $('#TestId').val();

                if (TestId != 0) {
                    $http.get('/api/ApiTestNV/GetData/', {
                        params: {
                            Compid: compid,

                            testid: TestId
                           


                        }
                    }).success(function (data) {
                        var RestGname = data[0].RestGroupName;


                        //  $('#TestCatId').val(TCatID);

                        if (RestGname != "") {
                            $scope.TestNVData = data;
                        }
                        else {
                            $scope.TestNVData = null;
                        }

                        $scope.loading = false;

                    });
                }
              



                if (data.RestGroupName != "") {
                    $("#RestId").val("");
                    $("#RestName").val("");
                    $('#RestGroupName').val("");
                    $('#RestMU').val("");
                    $('#ShowType').val("");
                    $('#Length').val("");
                    $('#Decimal').val("");
                    $('#NVALUE').val("");
                    $('#DVALUE').val("");
                    $('#SerialNo').val("");

                    //$scope.TestNVData.push({ ID: data.ID, RestGroupName: data.RestGroupName, RestMU: data.RestMU, ShowType: data.ShowType, Length: data.Length, Decimal: data.Decimal, NValue: data.NValue });
                } else {
                    $("#RestId").val("");
                    $("#RestName").val("");
                    $('#RestGroupName').val("");
                    $('#RestMU').val("");
                    $('#ShowType').val("");
                    $('#Length').val("");
                    $('#Decimal').val("");
                    $('#NVALUE').val("");
                    $('#DVALUE').val("");
                    $('#SerialNo').val("");

                    alert("duplicate name will not create");
                }

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        } else {
            $('#RestGroupName').val("");
            $('#RestMU').val("");
            $('#ShowType').val("");
            $('#Length').val("");
            $('#Decimal').val("");
            $('#NVALUE').val("");
            $('#DVALUE').val("");

            alert("Enter Test NV Master Data First");
        }

    };

    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;

    };

    $scope.toggleEdit_Cancel = function() {
        this.testitem.editMode = !this.testitem.editMode;

        var compid = $('#txtcompid').val();

        //var Restid = $('#RestId').val();
        var TestId = $('#TestId').val();


        $http.get('/api/ApiTestNV/GetData/', {
            params: {
                Compid: compid,

                testid: TestId
                //restid: Restid


            }
        }).success(function (data) {
            var RestGname = data[0].RestGroupName;


            //  $('#TestCatId').val(TCatID);

            if (RestGname != "") {
                $scope.TestNVData = data;
            }
            else {
                $scope.TestNVData = null;
            }

            $scope.loading = false;

        });

    };

    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;

        if (flag == 1) {

            this.testitem.RestId = $('#idgridRestId').val();

            this.testitem.RestName = $('#idgridRestName').val();
           
        } else {

        }

        var frien = this.testitem;
        this.testitem.COMPID = $('#txtcompid').val();
       
        this.testitem.TestId = $('#TestId').val();
       


        $http.post('/api/ApiTestNV/SaveData', this.testitem).success(function (data) {
            if (data.RestGroupName != "") {
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

    $scope.deleteItem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;
        $http.post('/api/ApiTestNV/DeleteData/', this.testitem).success(function (data) {

            $.each($scope.TestNVData, function (i) {
                if ($scope.TestNVData[i].ID === id) {
                    $scope.TestNVData.splice(i, 1);
                    return false;
                }
            });
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while Saving Friend! " + data;
            $scope.loading = false;

        });
    };



    $scope.x = function (value) {

        flag = 1;
      
        var compid = $('#txtcompid').val();
        var testid = $('#TestId').val();
        $('.gridRestName').val(value);
        $('.gridRestName').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: '/api/RestnmList',
                    type: 'GET',
                    cache: false,
                    data: { query: request.term, query2: compid, query3: testid },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.RestName,
                                value: item.RestName,
                                x: item.RestId
                            };
                        }));
                    }
                });

                
            },
            select: function (event, ui) {
                $('.gridRestName').val(ui.item.label);
                $('.gridRestId').val(ui.item.x);



                return true;
            },
            minLength: 1,



        });




    };

});