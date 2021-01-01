<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
   define("ROOT", $_SERVER["DOCUMENT_ROOT"] ."/bangiay/public/");
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  $category_child = $db->fetchAll("nhanhieu");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  /**
    * KIỂM TRA ID CỦA DANH MỤC
    **/
  
  $id = intval(getInput('id'));
  $idp = getInput('nameloaisp');
  //_debug($id);
   _debug($idp);
   // $ten_cha = $db->fetchNAME('category',$ten);
   // _debug($ten_cha);
  $ten_khongdau = $db->fetchNAME('loasp',$idp);
   _debug($ten_khongdau);

  $editproduct = $db->fetchID("thongtin_sanpham",$id);

  if(empty($editproduct)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_product.php");
  }


  if ($_SERVER["REQUEST_METHOD"] == "POST") {
      $data = [
        //name trong database bảng "category"
       "name" => postInput('name1'),
       "id_category_child" => postInput('id_category_child'),
       "price" => postInput('price'),
       "discount" => postInput('discount'),
       "content" => postInput('content'),
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
        $error['KhuyenMaI'] = "Mời bạn nhập đầy đủ giá sản phẩm!";
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
            $part = ROOT ."img/";
            $data['Hinh'] = $file_name;
            }
          }

          $id_update = $db->update("thongtin_sanpham", $data, array("id"=>$id));
          if($id_update > 0){
            move_uploaded_file($file_tmp, $part.$file_name );
            $_SESSION['success'] = "Cập nhật thành công";
            header("location:index_product.php");
          }
          else {
            $_SESSION['error'] = "Dữ liệu không thay đổi";
            header("location:index_product.php");
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
                        <a href="index_product.php">Danh mục sản phẩm</a>
                    </li>
                    <li class="breadcrumb-item active">Sửa sản phẩm</li>
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
                        <form class="form-horizontal" action="" method="POST" enctype="multipart/form-data">

                            <div class="form-group">
                              <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên thương hiệu</label>
                              <div class="col-sm-8">
                                 <select class="form-control col-md-8" name="id_parent" >
                                   <option value=""> - Mời bạn chọn tên thương hiệu - </option>
                                   <?php foreach ($category as $value): ?>
                                      <option value="<?php echo $value['id']?>" <?php echo $ten_khongdau['id'] == $value['id'] ? "selected = 'selected'" : '' ?>>
                                        <?php  echo $value['name'] ?>
                                      </option>
                                   <?php endforeach ?>
                                 </select>
                                <?php if (isset($error['id_loaisp'])): ?>
                                    <p class="text-danger"><?php echo $error['id_loaisp'] ?></p>
                                <?php endif ?>
                              </div>
                              
                            </div>
                           
                            <div class="form-group">
                              <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên danh mục</label>
                              <div class="col-sm-8">
                                 <select class="form-control col-md-8" name="id_category_child" >
                                   <option value=""> - Mời bạn chọn tên danh mục- </option>
                                   <?php foreach ($category_child as $value): ?>
                                      <option value="<?php echo $value['id']?>" <?php echo $editproduct['id_NH'] == $value['id'] ? "selected = 'selected'" : '' ?>>
                                        <?php  echo $value['name'] ?>
                                      </option>
                                   <?php endforeach ?>
                                 </select>
                                <?php if (isset($error['id_NH'])): ?>
                                    <p class="text-danger"><?php echo $error['id_NH'] ?></p>
                                <?php endif ?>
                              </div>
                              
                            </div>


                            <div class="form-group">
                                <label class="col-sm-2 control-label" for="exampleInputEmail1">Tên sản phẩm</label>
                                <div class="col-sm-8">
                                  <input type="text" name="name1" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="Tên sản phẩm" value="<?php echo $editproduct['name']?>">
                                  <?php if (isset($error['name1'])): ?>
                                      <p class="text-danger">
                                          <?php echo $error['name1'] ?>
                                      </p>
                                      <?php endif ?>
                                </div>
                            </div>
                            <div class="form-group">
                              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Giá sản phẩm</label>
                              <div class="col-sm-8">
                                <input type="number" name="price" class="form-control col-md-8" id="exampleInputdanhmuc" placeholder="9,000,000"  value="<?php echo $editproduct['price']?>">
                              <?php if (isset($error['GiaBan'])): ?>
                                <p class="text-danger"><?php echo $error['GiaBan'] ?></p>
                              <?php endif ?>
                              </div>
                            </div>

                            <div class="form-group">
                              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Giảm giá sản phẩm</label>
                              <div class="col-sm-3">
                                 <input type="number" name="discount" class="form-control" id="exampleInputdanhmuc" placeholder="10%" value="<?php echo $editproduct['discount']?>">
                                 <?php if (isset($error['KhuyenMaI'])): ?>
                                    <p class="text-danger"><?php echo $error['KhuyenMaI'] ?></p>
                                  <?php endif ?>
                              </div>
                              
                              <label  class="col-sm-2 control-label" for="exampleInputEmail1">Hình ảnh</label>
                              <div class="col-sm-3">
                                 <input type="file" name="image_list" class="form-control" id="exampleInputEmail1" >
                                 <?php if (isset($error['Hinh'])): ?>
                                    <p class="text-danger"><?php echo $error['Hinh'] ?></p>
                                  <?php endif ?>
                                  <img width="200px" height="200px" src="/pageadmin/imgs/sneakers/<?php echo $editproduct['Hinh']?>">
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