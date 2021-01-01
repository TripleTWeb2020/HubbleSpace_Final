<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  //$category = $db->fetchAll("category_child");
  //var_dump($category);
  // $sql = "SELECT category_child.*,category.name as namecate FROM category_child LEFT JOIN category ON category.id = category_child.id_parent";
  // $category = $db->fetchJone('category_child',$sql);
  $product = $db->fetchJoneONE('thongtin_sanpham');
  $thongtinkythuat= $db->fetchJoneONE('thongsokythuat')
 ?>
 <?php require_once __DIR__. "/../layout_header.php"; ?> 
<!--  -->
     <div id="content-wrapper">

      <div class="container-fluid">

         <!-- Breadcrumbs -->
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <a href="index_product.php">Danh mục</a>
            <a href="addproduct.php" class="btn btn-primary">Thêm mới sản phẩm</a>
          </li>
         
        </ol>
        <div class="clearfix">
          <?php if (isset($_SESSION['success']) ) :?>
              <div class="alert alert-success">
                <strong>Tốt lắm! </strong>
                <?php echo $_SESSION['success']; unset($_SESSION['success']); ?>
              </div>
            <?php endif ; ?>
          <?php if (isset($_SESSION['error']) ) :?>
              <div class="alert alert-danger">
                <strong>Ôi không! </strong>
                <?php echo $_SESSION['error']; unset($_SESSION['error']); ?>
              </div>
            <?php endif ; ?>
        </div>
        
        <!-- DataTables Example -->
        <div class="card mb-3">
          <div class="card-header">
            <i class="fas fa-table"></i>
            Danh sách danh mục hãng sản phẩm:</div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                  <tr>
                    <th>STT</th>
                    <th>Tên sản phẩm</th>
                    <th>Hình ảnh</th>
                      <th>Gía bán</th>
                      <th>Gía khuyến mãi</th>
                      <th>Loại sản phẩm</th>
                      <th>Nhãn hiệu</th>
                    <th >Thông tin</th>
                    <th>Tính năng</th>
                  </tr>
                </thead>
                <tbody >
                  <?php 
                    $stt = 1;
                    foreach ($product as $value): ?>
                        <tr>
                          <th><?php echo $stt ?></th>
                          <th><?php echo $value['TenSP'] ?></th>
                          <th><img width="200px" height="200px" src="/bangiay/public/sneakers/<?php echo $value['Hinh'] ?>"></th>
                            <th><?php echo number_format($value['GiaBan'])?>vnd</th>
                            <th><?php echo $value['KhuyenMai'] ?></th>
                            <th><?php echo $value['nameloaisp'] ?></th>
                            <th> <?php echo $value['namenhanhieu'] ?></th>
                          <td>



                              <?php foreach ($thongtinkythuat as $vlu): ?>
                              <p> <strong>Màu:</strong><?php echo $vlu['Màu Sắc'] ?></p>
                                  <p> <strong>Màn hình:</strong><?php echo $vlu['Màn hình'] ?></p>
                                  <p> <strong>Hệ điều hành:</strong><?php echo $vlu['Hệ điều hành'] ?></p>
                                  <p> <strong>CPU:</strong><?php echo $vlu['CPU'] ?></p>
                                  <p> <strong>RAM:</strong><?php echo $vlu['Ram'] ?></p>
                                  <p> <strong>Bộ nhớ:</strong><?php echo $vlu['Bộ nhớ'] ?></p>
                                  <p> <strong>Camera trước:</strong><?php echo $vlu['Camera trước'] ?></p>
                                  <p> <strong>Camera sau:</strong><?php echo $vlu['Camera sau'] ?></p>
                                  <p> <strong>Thẻ sim:</strong><?php echo $vlu['Thẻ sim'] ?></p>
                                  <p> <strong>Dung lượng pin:</strong><?php echo $vlu['Dung lượng pin'] ?></p>
                              <?php endforeach ?>

                          </td>
                          <th>
                            <a class="btn btn-primary" href="editproduct.php?id=<?php echo $value['id']?>&id_parent=<?php echo $value['id_NH']?>&namecate=<?php echo $value['nameloaisp']?>"><i class="fa fa-edit"> </i> Sửa</a>
                            <a class="btn btn-danger" href="deleteproduct.php?id=<?php echo $value['id']?>"><i class="fa fa-times"> </i> Xóa</a>
                          </th>
                        </tr>
                  <?php $stt++; endforeach ?>

                </tbody>


              </table>
            </div>
          </div>
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
