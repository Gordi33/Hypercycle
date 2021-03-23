#!python

# Initial settings

import numpy as np
import pandas as pd
import sys
import time
import random
import os
import csv
import matplotlib.pyplot as plt



class Hypercycle: 
	def __init__(self, p_colours, p_dim1, p_dim2): 
		print("Constructor called with", p_colours, "colours by", p_dim1, "x", p_dim2, "field.")
		self.__colours_number = p_colours
		self.__colours_list = list(range(1, self.__colours_number + 1))
		self.__colours_count = np.zeros(self.__colours_number)
		self.__colours_count_df = pd.DataFrame(self.__colours_count).transpose()
		self.__dim1 = p_dim1
		self.__dim2 = p_dim2
		self.__size = self.__dim1 * self.__dim2
		self.__colour_ratio = self.__size / self.__colours_number
		self.__colours_equal_distributed  = bool(np.where(self.__colour_ratio % int(self.__colour_ratio) == 0, 1, 0))
		self.__playing_ground = np.zeros((self.__dim1, self.__dim2))

	def __del__(self): 
		print("Destructor called.")

	def initialize_playing_ground(self):
		tmp_colour_list = self.__colours_list
		eq_col_in_list = list(int(self.__colour_ratio) * self.__colours_list)
		random.shuffle(eq_col_in_list)
		missing_col_size = self.__size - len(eq_col_in_list)
		random.shuffle(tmp_colour_list)
		tot_col = eq_col_in_list + tmp_colour_list[0:missing_col_size]
		self.__playing_ground = np.array(tot_col).reshape(self.__dim1, self.__dim2)
		self.compute_colours_count()

	def print_playing_ground(self): 
		print(self.__playing_ground)

	def plot_playing_ground(self): 
		X = self.get_playing_ground()
		fig = plt.figure(figsize=(8,6))
		plt.imshow(X)

	def plot_colours_count_path(self):
		myTitle = "Play with " + str(self.__colours_number) + " colours by " + str(self.__dim1) + " x " + str(self.__dim2) + " field"
		self.get_colours_count_df().plot(figsize=(14,6), title = myTitle)

	def get_playing_ground(self): 
		return self.__playing_ground 

	def set_playing_ground(self, p_playing_ground): 
		self.__playing_ground = p_playing_ground 

	def compute_colours_count(self): 
		self.__colours_count = np.zeros(self.__colours_number)
		for i in range(self.__dim1):
			for j in range(self.__dim2):
				index = self.__playing_ground[i-1][j-1] - 1
				self.__colours_count[index] += 1
		temp = pd.DataFrame(self.__colours_count).transpose()     
		self.__colours_count_df = pd.concat([self.__colours_count_df, temp]).reset_index(drop=True)

	def get_colours_count(self): 
		return self.__colours_count

	def get_colours_count_df(self): 
		return self.__colours_count_df
	
	def get_colours_number(self):
		return self.__colours_number

	def playing_move(self): 
		# create random field
		i = random.randint(1, self.__dim1) - 1
		j = random.randint(1, self.__dim2) - 1 

		self.__playing_ground[i][j] = 0 # remove colour
		go = True;

		while(go):
			# create second random field
			i2 = random.randint(1, self.__dim1) - 1
			j2 = random.randint(1, self.__dim2) - 1 

			# if neighour of first field equals second drawn field then substitute
			if j2 > 0 and self.__playing_ground[i2][j2-1] == self.__playing_ground[i2][j2]:
				go = False
			if j2 < self.__dim2 - 2 and self.__playing_ground[i2][j2+1] == self.__playing_ground[i2][j2]:
				go = False
			if i2 > 0 and self.__playing_ground[i2-1][j2] == self.__playing_ground[i2][j2]:
				go = False
			if i2 < self.__dim1 - 2 and self.__playing_ground[i2+1][j2] == self.__playing_ground[i2][j2]:
				go = False

			# place second colour in the first drawn field
			if go == False:
				self.__playing_ground[i][j] = self.__playing_ground[i2][j2]

		self.compute_colours_count()

	def game_has_finished(self): 
		for i in range(self.__colours_number):
			if self.__colours_count[i] == self.__size:
				return True
			return False

	def play(self, p_rounds = None): 
		self.initialize_playing_ground()

		if p_rounds is None:
			p_rounds = 10000

		run = 1
		while run < p_rounds:
			if run % 2500 == 0:
				print(run, "number of rounds completed (max default is", p_rounds, ")")
			self.playing_move()
			run += 1
			if self.game_has_finished():
				break
 