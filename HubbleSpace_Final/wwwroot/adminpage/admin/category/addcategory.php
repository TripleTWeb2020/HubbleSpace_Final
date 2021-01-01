<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);
  if ($_SERVER["REQUEST_METHOD"] == "POST") {
      $data = [
        //name trong database bảng "category"
       "name" => postInput('name1')
       ];
      $error = [];
      if (postInput('name1') == '') {
        $error['name1'] = "Mời bạn nhập đầy đủ tên danh mục!";
      }
      if (empty($error)) {
        //BĂT LỖI THÊM TRÙNG DANH MỤC
        $isset = $db->fetchOne("loaisp","name = '".$data['name']."' ");
        // var_dump($isset);
        if (count($isset)>0) {
          $_SESSION['error'] = " Tên danh mục đã tồn tại ! ";
        }
        else{
            $id_insert = $db->insert("loaisp", $data);
          if($id_insert > 0){
            $_SESSION['success'] = "Thêm mới thành công";
            header("location:index_category.php");
          }
          else {
            $_SESSION['error'] = "Thêm mới thất bại";
          }
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
            <a href="index_category.php">Danh sách</a>
          </li>
          <li class="breadcrumb-item active">Thêm mới loại sản phẩm</li>
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
           <form class="form-horizontal" action="" method="POST">
            <div class="form-group">
              <label for="exampleInputEmail1">Loại Sản Phẩm</label>
              <input type="text" name="name1" class="form-control" id="exampleInputdanhmuc" placeholder="Loại sản phẩm">
              <?php if (isset($error['name1'])): ?>
                <p class="text-danger"><?php echo $error['name1'] ?></p>
              <?php endif ?>
              
            </div>
            <button  type="submit" class="btn btn-success" >Lưu</button>

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

  </div>
  <!-- /#wrapper -->

  <!-- Scroll to Top Button-->
  <?php require_once __DIR__. "/../layout_footer.php"; ?> 
