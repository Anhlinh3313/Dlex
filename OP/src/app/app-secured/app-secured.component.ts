import { trigger, state, style, animate, transition } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { MenuService } from '../shared/services/local/menu.service';

@Component({
  selector: 'app-app-secured',
  templateUrl: './app-secured.component.html',
  styleUrls: ['./app-secured.component.scss'],
  animations: [
    trigger('mask-anim', [
      state('void', style({
        opacity: 0
      })),
      state('visible', style({
        opacity: 0.8
      })),
      transition('* => *', animate('250ms cubic-bezier(0, 0, 0.2, 1)'))
    ])
  ]
})
export class AppSecuredComponent implements OnInit {
  horizontalMenu: boolean;

  darkMode = false;
  menuColorMode = 'light';
  menuColor = 'layout-menu-light';
  themeColor = 'blue';
  layoutColor = 'blue';
  rightPanelClick: boolean;
  rightPanelActive: boolean;
  menuClick: boolean;
  staticMenuActive: boolean;
  menuMobileActive: boolean;
  megaMenuClick: boolean;
  megaMenuActive: boolean;
  megaMenuMobileClick: boolean;
  megaMenuMobileActive: boolean;
  topbarItemClick: boolean;
  topbarMobileMenuClick: boolean;
  topbarMobileMenuActive: boolean;
  configDialogActive: boolean;
  sidebarActive: boolean;
  activeTopbarItem: any;
  topbarMenuActive: boolean;
  menuHoverActive: boolean;

  constructor(
    private menuService: MenuService,
  ) { }

  ngOnInit(): void {
    this.staticMenuActive = true;
  }

  onLayoutClick(): any {
    if (!this.topbarItemClick) {
      this.activeTopbarItem = null;
      this.topbarMenuActive = false;
    }

    if (!this.rightPanelClick) {
      this.rightPanelActive = false;
    }

    if (!this.megaMenuClick) {
      this.megaMenuActive = false;
    }

    if (!this.megaMenuMobileClick) {
      this.megaMenuMobileActive = false;
    }

    if (!this.menuClick) {
      if (this.isHorizontal()) {
        this.menuService.reset();
      }
      if (this.menuMobileActive) {
        this.menuMobileActive = false;
      }
      this.menuHoverActive = false;
    }
    this.menuClick = false;
    this.topbarItemClick = false;
    this.megaMenuClick = false;
    this.megaMenuMobileClick = false;
    this.rightPanelClick = false;
  }

  onMegaMenuButtonClick(event): any {
    this.megaMenuClick = true;
    this.megaMenuActive = !this.megaMenuActive;
    event.preventDefault();
  }

  onMegaMenuClick(event): any {
    this.megaMenuClick = true;
    event.preventDefault();
  }

  onTopbarItemClick(event, item): any {
    this.topbarItemClick = true;
    if (this.activeTopbarItem === item) {
      this.activeTopbarItem = null;
    } else {
      this.activeTopbarItem = item;
    }
    event.preventDefault();
  }

  onRightPanelButtonClick(event): any {
    this.rightPanelClick = true;
    this.rightPanelActive = !this.rightPanelActive;
    event.preventDefault();
  }

  onRightPanelClose(event): any {
    this.rightPanelActive = false;
    this.rightPanelClick = false;
    event.preventDefault();
  }

  onRightPanelClick(event): any {
    this.rightPanelClick = true;
    event.preventDefault();
  }

  onTopbarMobileMenuButtonClick(event): any {
    this.topbarMobileMenuClick = true;
    this.topbarMobileMenuActive = !this.topbarMobileMenuActive;
    event.preventDefault();
  }

  onMegaMenuMobileButtonClick(event): any {
    this.megaMenuMobileClick = true;
    this.megaMenuMobileActive = !this.megaMenuMobileActive;
    event.preventDefault();
  }

  onMenuButtonClick(event): any {
    this.menuClick = true;
    this.topbarMenuActive = false;
    if (this.isMobile()) {
      this.menuMobileActive = !this.menuMobileActive;
    }
    event.preventDefault();
  }

  onSidebarClick(event: Event): any {
    this.menuClick = true;
  }

  onToggleMenuClick(event: Event): any {
    this.staticMenuActive = !this.staticMenuActive;
    event.preventDefault();
  }

  isDesktop(): any {
    return window.innerWidth > 991;
  }

  isMobile(): any {
    return window.innerWidth <= 991;
  }

  isHorizontal(): any {
    return this.horizontalMenu === true;
  }
}
