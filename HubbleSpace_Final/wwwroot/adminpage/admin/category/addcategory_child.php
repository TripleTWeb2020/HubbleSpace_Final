<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);
  /**
    * LẤY RA DANH SÁCH TÊN THƯƠNG HIỆU
    **/

  if ($_SERVER["REQUEST_METHOD"] == "POST") {
      $data = [
        //name trong database bảng "category"
       "name" => postInput('name1'),

       "id_NH" => postInput('id_NH')
       ];
      $error = [];
      if (postInput('name1') == '') {
        $error['name1'] = "Mời bạn nhập đầy đủ tên danh mục!";
      }
      if (empty($error)) {
        // if(isset($_FILES['name1'])){
        //   $file_name = $_FILES['name1']['name'];
        // }
        $id_insert = $db->insert("nhanhieu",$data);
        if($id_insert>0){
          $_SESSION['success'] = "Thêm mới thành công";
            header("location:index_category_child.php");
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
            <a href="index_category_child.php">Danh sách</a>
          </li>
          <li class="breadcrumb-item active">Thêm mới nhãn hiệu</li>
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
              <label class="col-sm-2 control-label" for="exampleInputEmail1">Loại sản phầm</label>
              <div class="col-sm-8">
                 <select class="form-control col-md-8" name="id_parent">
                   <option value=""> - Mời bạn chọn loại sản phẩm - </option>
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
              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Tên nhãn hiệu</label>
              <div class="col-sm-8">
                <input type="text" name="name1" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="Tên nhãn hiệu">
              <?php if (isset($error['name1'])): ?>
                <p class="text-danger"><?php echo $error['name1'] ?></p>
              <?php endif ?>
              </div>
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
