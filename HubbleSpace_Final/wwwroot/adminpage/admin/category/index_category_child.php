<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  //$category = $db->fetchAll("category_child");
  //var_dump($category);
  // $sql = "SELECT category_child.*,category.name as namecate FROM category_child LEFT JOIN category ON category.id = category_child.id_parent";
  // $category = $db->fetchJone('category_child',$sql);
  $category = $db->fetchJone('nhanhieu');
 ?>
 <?php require_once __DIR__. "/../layout_header.php"; ?> 
<!--  -->
     <div id="content-wrapper">

      <div class="container-fluid">

         <!-- Breadcrumbs -->
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <a href="index_category.php">Danh sách</a>
            <a href="addcategory_child.php" class="btn btn-primary">Thêm mới nhãn hiệu</a>
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
            Danh sách nhãn hiệu:</div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                  <tr>
                    <th>STT</th>
                    <th>Tên</th>
                    <th>Nhãn hiệu</th>

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
                          <th><?php echo $value['namecate'] ?></th>

                          <th>
                            <a class="btn btn-primary" href="editcategory_child.php?id=<?php echo $value['id']?>"><i class="fa fa-edit"> </i> Sửa</a>
                            <a class="btn btn-danger" href="deletecategory_child.php?id=<?php echo $value['id']?>"><i class="fa fa-times"> </i> Xóa</a>
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
