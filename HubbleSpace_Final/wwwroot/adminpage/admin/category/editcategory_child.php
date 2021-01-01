<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  /**
    * KIỂM TRA ID CỦA DANH MỤC
    **/
  $id = intval(getInput('id'));

  $editcategory_child = $db->fetchID("nhanhieu",$id);
  if(empty($editcategory_child)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_category_child.php");
  }


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
      if (postInput('id_NH') == '') {
        $error['id_NH'] = "Mời bạn chọn tên thương hiệu!";
      }

      if (empty($error)) {
          $id_update = $db->update("nhanhieu", $data, array("id"=>$id));
          if($id_update > 0){
            $_SESSION['success'] = "Cập nhật thành công";
            header("location:index_category_child.php");
          }
          else {
            $_SESSION['error'] = "Dữ liệu không thay đổi";
            header("location:index_category_child.php");
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
                    <li class="breadcrumb-item active">Sửa nhãn hiệu</li>
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
                              <label class="col-sm-2 control-label" for="exampleInputEmail1">Loại sản phẩm</label>
                              <div class="col-sm-8">
                                 <select class="form-control col-md-8" name="id_parent" >
                                   <option value=""> - Mời bạn chọn loại sản phầm - </option>
                                   <?php foreach ($category as $value): ?>
                                      <option value="<?php echo $value['id']?>" <?php echo $editcategory_child['id_parent'] == $value['id'] ? "selected = 'selected'" : '' ?>>
                                        <?php  echo $value['name'] ?>
                                      </option>
                                   <?php endforeach ?>
                                 </select>
                                <?php if (isset($error['id_parent'])): ?>
                                    <p class="text-danger"><?php echo $error['id_parent'] ?></p>
                                <?php endif ?>
                              </div>
                              
                            </div>


                            <div class="form-group">
                                <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên nhãn hiệu</label>
                                <div class="col-sm-8">
                                  <input type="text" name="name1" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="Tên nhãn hiệu" value="<?php echo $editcategory_child['name']?>">
                                  <?php if (isset($error['name1'])): ?>
                                      <p class="text-danger">
                                          <?php echo $error['name1'] ?>
                                      </p>
                                      <?php endif ?>
                                </div>
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