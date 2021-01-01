<option value=""> - Mời bạn chọn tên danh mục - </option>
<?php 
	//session_start();
 	require_once __DIR__. "/../../../model/Database1.php";
  	require_once __DIR__. "/../../../controller/funciton.php";
  	$db = new Database1();
  	$key = $_POST['id_thuonghieu'];
  	$Load_thuonghieu = $db->get_danhmuc($key);
  	//_debug($Load_thuonghieu);
  	?>
  		<?php foreach ($Load_thuonghieu as $value): ?>
  			
  				<option value="<?php echo $value['id']?>"><?php if (isset($value['name'])) {
  					?>
					<?php echo $value['name']; ?>
  					<?php
  				} ?></option>
  		
				

  		<?php endforeach ?>
  	<?php        
 ?>