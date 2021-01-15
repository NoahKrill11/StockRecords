from selenium import webdriver
from webdriver_manager.chrome import ChromeDriverManager
from webdriver_manager.utils import ChromeType
import time
import string
from datetime import datetime
browser = webdriver.Chrome(ChromeDriverManager().install())

browser.get('https://localhost:44338/Admin')
i = 1
while i!=0:
	if int(time.strftime("%H")) > 9 and int(time.strftime("%H")) < 16:
		button = browser.find_element_by_id('Button1')
		button.click()
	
	
