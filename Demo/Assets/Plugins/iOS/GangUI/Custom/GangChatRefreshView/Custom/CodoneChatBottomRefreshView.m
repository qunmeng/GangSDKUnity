//
//  CodoneNormalLoadView.m
//  RefreshTest
//
//  Created by codone on 2017/6/12.
//  Copyright © 2017年 codone. All rights reserved.
//

#import "CodoneChatBottomRefreshView.h"
#import "GangTools.h"
#import "GangUIFont.h"

@implementation CodoneChatBottomRefreshView

-(void)awakeFromNib{
    [super awakeFromNib];
    self.label.textColor = [UIColor colorFromHexRGB:GangColor_refresh_text];
    self.indicator.color = [UIColor colorFromHexRGB:GangColor_refresh_text];
}

/*
// Only override drawRect: if you perform custom drawing.
// An empty implementation adversely affects performance during animation.
- (void)drawRect:(CGRect)rect {
    // Drawing code
}
*/

#pragma override
-(void)hasChangeToState:(CodoneRefreshState)state{
    switch (state) {
        case State_CanPullOrClick:
            self.label.text = [GangTools getLocalizationOfKey:@"点击或上拉刷新"];
            self.indicator.hidden = YES;
            [self.indicator stopAnimating];
            break;
        case State_CanPull:
            self.label.text = [GangTools getLocalizationOfKey:@"上拉刷新"];
            self.indicator.hidden = YES;
            [self.indicator stopAnimating];
            break;
        case State_ReleaseToLoad:
            self.label.text = [GangTools getLocalizationOfKey:@"松开刷新"];
            self.indicator.hidden = YES;
            [self.indicator stopAnimating];
            break;
        case State_Refreshing:
            self.label.text = [GangTools getLocalizationOfKey:@"正在刷新"];
            self.indicator.hidden = NO;
            [self.indicator startAnimating];
            break;
        case State_NoMoreData:
            self.label.text = [GangTools getLocalizationOfKey:@"全部加载完成"];
            self.indicator.hidden = YES;
            [self.indicator stopAnimating];
            break;
            
        default:
            break;
    }
}

@end
