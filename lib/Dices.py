#!python

# Initial settings

import sys
import numpy as np
import pandas as pd
import seaborn as sns
from scipy import stats
import statistics
from matplotlib import pyplot as plt
import time
import random
import os
import csv



class Dices_Game:
    def __init__ (self):
        self.__return_to_player = 0.9 
        self.__number_of_dices = 1
        self.__starting_balance = 20
        self.__dices_prizes = [1,2,3,4,5,6]
        self.__fair_price = 0
        self.__margined_price = 0        
        self.__throwing_result = [0 for i in range(self.__number_of_dices)]
        self.__played_till_zero_container = []
        self.__dices_distribution = None
        
    def __del__(self):
        pass

    def set_number_of_dices(self, number_of_dices):
        self.__number_of_dices = number_of_dices 

    def set_RTP(self, return_to_player):
        self.__return_to_player = return_to_player

    def set_dices_prizes(self, dices_prizes):
        self.__dices_prizes = dices_prizes

    def throw_dices(self):
        self.__throwing_result = [randint(1, 6) for i in range(self.__number_of_dices)]

    def set_starting_balance(self, starting_balance):
        self.__starting_balance = starting_balance

    def set_fair_price(self):
        temp = [self.__dices_prizes[i-1] for i in (self.__throwing_result)] 
        self.__fair_price = np.prod(temp)        

    def set_margined_price(self):
        self.set_fair_price()
        self.__margined_price = self.__return_to_player * (self.__fair_price * 1.0)           

    def get_played_till_zero_container(self):
        return self.__played_till_zero_container

    def get_dices_distribution(self):
        return self.__dices_distribution

    def play_till_zero(self):
        balance = self.__starting_balance
        games = 0
        while balance > 0:
            games    += 1
            balance  -= 1
            self.throw_dices()
            self.set_margined_price()
            balance  += self.__margined_price
            #print(games, "\t", self.__margined_price, "\t", balance, "\t", self.__throwing_result)
                
        self.__played_till_zero_container.append(games)

    def play_till_zero_n_times(self, n):
        self.__played_till_zero_container = []
        
        for i in range(1, n+1):
            self.play_till_zero()
            
            if i%(n/4)==0:
                print(100* (i)/n, " %")
                
        self.__dices_distribution = [0] * (max(self.__played_till_zero_container) + 1)

        for i in range(len(self.__played_till_zero_container)):
            self.__dices_distribution[self.__played_till_zero_container[i]] += 1

    def plot_dices_distribution(self, min_x = 0, max_x = 10000):
        plt.plot(self.__dices_distribution)
        plt.xlim(min_x, max_x)
        plt.xscale("log")
     
    def get_dices_distribution_statistics(self):
        _temp1 = pd.DataFrame(self.__played_till_zero_container)
        _temp1 = _temp1.rename(columns={0: "Statistic"})
        _temp2 = _temp1.describe()
        _temp3 = pd.DataFrame(self.__played_till_zero_container).agg({0: ['median']})
        _temp3 = _temp3.rename(columns={0: "Statistic"})
        _temp4 = pd.concat([_temp2, _temp3])
        _temp4.loc['mode'] = [statistics.mode(self.__played_till_zero_container)]
        return _temp4
