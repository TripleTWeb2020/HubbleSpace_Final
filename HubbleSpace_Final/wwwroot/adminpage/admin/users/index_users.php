<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $user = $db->fetchAll("users");
  //var_dump($user);

 ?>
 <?php require_once __DIR__. "/../layout_header.php"; ?> 
<!--  -->
     <div id="content-wrapper">

      <div class="container-fluid">

         <!-- Breadcrumbs -->
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <a href="index_users.php">Tài khoản</a>
            
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
            Danh sách tài khoản:</div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                  <tr>
                    <th>STT</th>
                    <th>Tên Tài Khoản</th>
                    <th>Họ Tên</th>
                    <th>Mật Khẩu</th>
                    <th>Email</th>
                    <th>Điện Thoại</th>
                    <th>Địa Chỉ</th>
                    <th>Tính Năng</th>
                  </tr>
                </thead>
                <tbody >
                  <?php 
                    $stt = 1;
                    foreach ($user as $value): ?>
                        <tr>
                          <th><?php echo $stt ?></th>
                          <th><?php echo $value['TenUser'] ?></th>
                          <th><?php echo $value['HoTen'] ?></th>
                          <th><?php echo $value['MatKhau']?></th>
                          <th><?php echo $value['Email'] ?></th>
                          <th><?php echo $value['Phone'] ?></th>
                          <th><?php echo $value['DiaChi']?></th>
                         
                          <th>
                            <a class="btn btn-danger" href="deleteusers.php?id=<?php echo $value['id']?>"><i class="fa fa-times"> </i> Xóa</a>
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
