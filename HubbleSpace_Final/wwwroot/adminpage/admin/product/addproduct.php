<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  define("ROOT", $_SERVER["DOCUMENT_ROOT"] ."/adminpage/admin/");
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  $category_child = $db->fetchAll("nhanhieu");
  //var_dump($category);
  
  
  /**
    * LẤY RA DANH SÁCH TÊN THƯƠNG HIỆU
    **/
  

  if ($_SERVER["REQUEST_METHOD"] == "POST") {
      $data = [
        //name trong database bảng "category"
       "name" => postInput('name1'),
       "id_NH" => postInput('id_NH'),
       "GiaBan" => postInput('GiaBan'),
       "KhuyenMai" => postInput('KhuyenMai'),

       ];
      $error = [];
      if (postInput('name1') == '') {
        $error['name1'] = "Mời bạn nhập đầy đủ tên sản phẩm!";
      }
      if (postInput('id_loaisp') == '') {
        $error['id_loaisp'] = "Mời bạn chọn tên thương hiệu!";
      }
      if (postInput('id_NH') == '') {
        $error['id_NH'] = "Mời bạn chọn tên danh mục!";
      }
      if (postInput('GiaBan') == '') {
        $error['GiaBan'] = "Mời bạn nhập đầy đủ giá sản phẩm!";
      }
      if (postInput('KhuyenMai') == '') {
        $error['KhuyenMai'] = "Mời bạn nhập đầy đủ giá sản phẩm!";
      }

      if(!isset($_FILES['Hinh'])){
        $error['Hinh'] = "Mời bạn chọn đầy đủ hình ảnh!";
      }
      if (empty($error)) {
        if(isset($_FILES['Hinh'])){
          $file_name = $_FILES['Hinh']['name'];
          $file_tmp = $_FILES['Hinh']['tmp_name'];
          $file_type = $_FILES['Hinh']['type'];
          $file_erro = $_FILES['Hinh']['error'];
          if($file_erro == 0){
            $part = ROOT ."imgs/";
            $data['Hinh'] = $file_name;
          }
        }
        $id_insert = $db->insert("thongtin_sanpham",$data);
        if($id_insert>0){
          move_uploaded_file($file_tmp, $part.$file_name );
          $_SESSION['success'] = "Thêm mới thành công";
            header("location:index_product.php");
          }
          else {
            $_SESSION['error'] = "Thêm mới thất bại";
          }
      }
  }
 ?>
 <?php require_once __DIR__. "/../layout_header.php"; ?> 
<!--  -->
     <div id="content-wrapper">

      <div class="container-fluid">

         <!-- Breadcrumbs -->
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <a href="index_product.php">Sản phẩm</a>
          </li>
          <li class="breadcrumb-item active">Thêm mới sản phẩm</li>
        </ol>
        
         <div class="clearfix">
          <?php if (isset($_SESSION['error']) ) :?>
              <div class="alert alert-danger">
                <strong>Ôi không! </strong>
                <?php echo $_SESSION['error']; unset($_SESSION['error']); ?>
              </div>
            <?php endif ; ?>
        </div>
        <div class="row">
          <div class="col-md-12">
           <form class="form-horizontal" action="" method="POST" enctype="multipart/form-data">
            <div class="form-group">
              <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên thương hiệu</label>
              <div class="col-sm-8">
                 <select class="form-control col-md-8" name="id_loaisp" id="id_loaisp">
                   <option value=""> - Mời bạn chọn tên thương hiệu - </option>
                   <?php foreach ($category as $value): ?>
                      <option value="<?php echo $value['id']?>"><?php  echo $value['name'] ?></option>
                   <?php endforeach ?>
                 </select>
                <?php if (isset($error['id_parent'])): ?>
                    <p class="text-danger"><?php echo $error['id_parent'] ?></p>
                <?php endif ?>
              </div>
              
            </div>

            <div class="form-group">
              <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên danh mục</label>
              <div class="col-sm-8">
                 <select class="form-control col-md-8" name="id_NH" id="id_NH">
                   <option value=""> - Mời bạn chọn tên danh mục - </option>
                  <?php foreach ($nhanhieu as $value): ?>
                      <option value="<?php echo $value['id']?>"><?php  echo $value['name'] ?></option>
                   <?php endforeach ?> 
                 </select>
                <?php if (isset($error['id_NH'])): ?>
                    <p class="text-danger"><?php echo $error['id_NH'] ?></p>
                <?php endif ?>
              </div>
              
            </div>
             
                 <!--   <script type="text/javascript">
                    $(document).ready(function(){
                      $("#id_parent").change(function(){
                        var id_thuonghieu = $(this).val();
                       alert(id_thuonghieu);
                        $.post("Load_thuonghieu.php",{"id_thuonghieu":id_thuonghieu},function (data) {
                          /* body... */
                          $("#id_category_child").html(data);
                        });
                      });
                    });
                  </script> -->

            

            
           <!--  
            <script type="text/javascript">
              jQuery(document).ready(function($) {
                  $("#id_parent").change(function(envent){
                    var id_thuonghieu = $("#id_parent").val();
                    // alert(id_thuonghieu);
                    $.post('Load_thuonghieu.php', {"id_thuonghieu":id_thuonghieu} ,function(data){
                        $("#id_category_child").html(data);
                    });
                  });
              });
            </script> -->


            <div class="form-group">
              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Tên sản phẩm</label>
              <div class="col-sm-8">
                <input type="text" name="name1" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="Tên sản phẩm">
              <?php if (isset($error['name1'])): ?>
                <p class="text-danger"><?php echo $error['name1'] ?></p>
              <?php endif ?>
              </div>
            </div>
            
            <div class="form-group">
              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Giá sản phẩm</label>
              <div class="col-sm-8">
                <input type="number" name="price" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="9,000,000 ">
              <?php if (isset($error['GiaBan'])): ?>
                <p class="text-danger"><?php echo $error['GiaBan'] ?></p>
              <?php endif ?>
              </div>
            </div>

            <div class="form-group">
              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Giảm giá sản phẩm</label>
              <div class="col-sm-3">
                 <input type="number" name="discount" class="form-control" id="exampleInputdanhmuc" placeholder="10%">
                 <?php if (isset($error['KhuyenMai'])): ?>
                    <p class="text-danger"><?php echo $error['KhuyenMai'] ?></p>
                  <?php endif ?>
              </div>
              
              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Hình ảnh</label>
              <div class="col-sm-3">
                 <input type="file" name="image_list" class="form-control" id="exampleInputEmail1" >
                 <?php if (isset($error['Hinh'])): ?>
                    <p class="text-danger"><?php echo $error['Hinh'] ?></p>
                  <?php endif ?>
              </div>
               
            </div>


            
            <div class="form-group">
              <div class="col-sm-2 control-label">
                <button type="submit" class="btn btn-success" >Lưu</button>
              </div>
            
            </div>
          </form>
          </div>
        
        </div>
        <br>
        <!-- DataTables Example -->
        <div class="card mb-3">
          <div class="card-header">
              <div class="card-footer small text-muted">Updated yesterday at 11:59 PM</div>
        </div>
        </div>
      </div>
    
      
      




    
    <!-- /.content-wrapper -->
<!--  -->

  <!-- /#wrapper -->

  <!-- Scroll to Top Button-->
  <?php require_once __DIR__. "/../layout_footer.php"; ?> 
     