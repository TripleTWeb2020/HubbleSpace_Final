<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);

 ?>
 <?php require_once __DIR__. "/../layout_header.php"; ?> 
<!--  -->
     <div id="content-wrapper">

      <div class="container-fluid">

         <!-- Breadcrumbs -->
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <a href="index_category.php">Danh sách</a>
            <a href="addcategory.php" class="btn btn-primary">Thêm mới loại sản phẩm</a>
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
            Danh sách loại sản phẩm:</div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                  <tr>
                    <th>STT</th>
                    <th>Tên Loại Sản Phẩm</th>
                    <th>Hiển thị</th>
                    <th>Tính năng</th>
                  </tr>
                </thead>
                <tbody >
                  <?php 
                    $stt = 1;
                    foreach ($category as $value): ?>
                        <tr>
                          <th><?php echo $stt ?></th>
                          <th><?php echo $value['name'] ?></th>
                          <th>
                            <a href="home_hienthi.php?id=<?php echo $value['id']?>" class="btn btn-xs <?php echo $value['home_hienthi'] == 1 ? 'btn-info' : 'btn-danger' ?>" >
                              <?php echo $value['home_hienthi'] == 1 ? ' Hiển thị ' : ' Không ' ?>
                            </a>
                          </th>
                          <th>
                            <a class="btn btn-primary" href="editcategory.php?id=<?php echo $value['id']?>"><i class="fa fa-edit"> </i> Sửa</a>
                            <a class="btn btn-danger" href="deletecategory.php?id=<?php echo $value['id']?>"><i class="fa fa-times"> </i> Xóa</a>
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
