<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  $id = intval(getInput('id'));

  $editcategory = $db->fetchID("loaisp",$id);
  if(empty($editcategory)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_category.php");
  }


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
        if ($editcategory['name'] != $data['name']) {
          $isset = $db->fetchOne("loaisp","name = '".$data['name']."' ");
          if (count($isset)>0) {
            $_SESSION['error1'] = " Tên danh mục đã tồn tại ! ";
          }
          else{
            $id_update = $db->update("loaisp", $data, array("id"=>$id));
            if($id_update > 0){
              $_SESSION['success'] = "Cập nhật thành công";
              header("location:index_category.php");
            }
            else {
              $_SESSION['error'] = "Dữ liệu không thay đổi";
                header("location:index_category.php");
            }
          }
        } 
        else {
           $id_update = $db->update("loaisp", $data, array("id"=>$id));
            if($id_update > 0){
              $_SESSION['success'] = "Cập nhật thành công";
              header("location:index_category.php");
            }
            else {
              $_SESSION['error'] = "Dữ liệu không thay đổi";
              header("location:index_category.php");
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
                        <a href="index_category_child.php">Loại sản phầm</a>
                    </li>
                    <li class="breadcrumb-item active">Sửa Loại Sản Phẩm</li>
                </ol>
                <div class="clearfix">
                  <?php if (isset($_SESSION['error1']) ) :?>
                      <div class="alert alert-danger">
                        <strong>Ôi không! </strong>
                        <?php echo $_SESSION['error1']; unset($_SESSION['error1']); ?>
                      </div>
                    <?php endif ; ?>
                </div>
                

                <div class="row">
                    <div class="col-md-12">
                        <form class="form-horizontal" action="" method="POST">
                            <div class="form-group">
                                <label for="exampleInputEmail1">Loại sản phẩm</label>
                                <input type="text" name="name1" class="form-control" id="exampleInputdanhmuc" placeholder="Loại sản phầm" value="<?php echo $editcategory['name']?>">
                                <?php if (isset($error['name1'])): ?>
                                    <p class="text-danger">
                                        <?php echo $error['name1'] ?>
                                    </p>
                                    <?php endif ?>

                            </div>
                            <button type="submit" class="btn btn-success">Lưu</button>

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