<?php include "header.php";?>
<section class="fow-fluid" id="contact">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                <div class="col-md-5">
                <h2>Place your message</h2>
                <form action="" method="">
                    <p><input type="text" class="form-control" placeholder="Enter your name"></p>
                    <p><input type="text" class="form-control" placeholder="Enter your email"></p>
                    <p><input type="text" class="form-control" placeholder="Enter your subject"></p>
                    <p><textarea name="" id="" cols="30" rows="10" class="form-control" placeholder="Enter your message"></textarea></p>
                    <input type="submit" class="btn" value="Send">
                    
                </form>
                
                </div> <!-- contact form end -->
                <div class="col-md-4 pull-right" id="address">
                    <h2>Contact Details</h2>
                    <a href="#">Dhaka Office</a>
                    <p>House# 99/3, Flat# A2, Park Road, </p>
                    <p>East Namapara, Khilkhet, Dhaka, Bangladesh.</p>
                    <a href="#">Chittagong Office</a>
                    <p>1047, O.R. Nizam Road,</p>
                    <p>Suborna R/A, Chittagong-4000, Bangladesh.</p>
                    <h4>Contact No</h4>
                    <p>+880-1925 444777, +880-1725 388391</p>
                    <p>+880-1925 444999, +880-1816 910849</p>
                    <h4>E-mail:</h4>
                    <p>info@alchemy-bd.com</p>
                    <p>alchemysoftware@yahoo.com</p>
                </div>
            </div>
        </div>
        </div>
    </section>
    <section class="row-fluid">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                <div class="col-md-6">
                    <h2>Location of Chittagong Office</h2>
                   <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<div style="overflow:hidden;height:300px;width:99%;">
    <div id="gmap_canvas1" style="height:300px;width:99%;"></div>
    <style>
    #gmap_canvas img {
        max-width: none!important;
        background: none!important
    }
    </style><a class="google-map-code" href="http://wordpress-themes.org" id="get-map-data">wordpress</a>
</div>
<script type="text/javascript">
function init_map() {
    var myOptions = {
        zoom: 16,
        center: new google.maps.LatLng(22.359960451989544, 91.82734947686845),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("gmap_canvas1"), myOptions);
    marker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(22.359960451989544, 91.82734947686845)
    });
    infowindow = new google.maps.InfoWindow({
        content: "<b>1047, O. R. NIZAM ROAD, 1ST FLOOR,</b><br/>GOLPAHAR, CHITTAGONG<br/>4000 CHITTAGONG"
    });
    google.maps.event.addListener(marker, "click", function() {
        infowindow.open(map, marker);
    });
    infowindow.open(map, marker);
}
google.maps.event.addDomListener(window, 'load', init_map);
</script>

                    <br>
                </div>

                <div class="col-md-6">
                    <h2>Location of Dhaka Office</h2>
                    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<div style="overflow:hidden;height:300px;width:99%;">
    <div id="gmap_canvas2" style="height:300px;width:99%;"></div>
    <style>
    #gmap_canvas img {
        max-width: none!important;
        background: none!important
    }
    </style><a class="google-map-code" href="http://wordpress-themes.org" id="get-map-data">wordpress</a>
</div>
<script type="text/javascript">
function init_map() {
    var myOptions = {
        zoom: 15,
        center: new google.maps.LatLng(23.83022309847922, 90.43164484421084),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("gmap_canvas2"), myOptions);
    marker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(23.83022309847922, 90.43164484421084)
    });
    infowindow = new google.maps.InfoWindow({
        content: "<b>House# 99/3, Flat# A2, Park Road</b><br/>East Namapara, Khilkhet, Dhaka, Bangladesh.<br/> Dhaka"
    });
    google.maps.event.addListener(marker, "click", function() {
        infowindow.open(map, marker);
    });
    infowindow.open(map, marker);
}
google.maps.event.addDomListener(window, 'load', init_map);
</script>


                    <br>
                </div>
            </div>
        </div>
        </div>
    </section>
<?php include "footer.php";?>